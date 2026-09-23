using System;
using System.Collections.Generic;
using UnityEngine;

// Queries the real collision shapes without moving the live plane or simulating the level.
public sealed class PlaneTrajectory
{
    public enum ContactKind { Ignore, Stop, Crash, Ground, DangerousGround }

    public struct Result
    {
        public bool crash;
        public Vector2 point;
        public Vector2 normal;
    }

    private readonly List<Collider2D> colliders = new List<Collider2D>();
    private readonly List<Collider2D> overlaps = new List<Collider2D>();
    private readonly List<RaycastHit2D> hits = new List<RaycastHit2D>();
    private readonly Dictionary<Collider2D, Vector2> initialContacts = new Dictionary<Collider2D, Vector2>();
    private readonly HashSet<Collider2D> stillOverlapping = new HashSet<Collider2D>();
    private readonly List<Collider2D> exited = new List<Collider2D>();

    public Result Predict(Rigidbody2D body, Vector2 impulse, float gravity, float damping,
        float duration, float maxDistance, List<Vector3> points,
        Func<Collider2D, ContactKind> classify, Func<Vector2, bool> outsideBounds = null)
    {
        points.Clear();
        initialContacts.Clear();
        body.GetAttachedColliders(colliders);
        Vector2 position = body.position;
        float angle = body.rotation;
        Vector2 velocity = body.linearVelocity + impulse / Mathf.Max(body.mass, 0.0001f);
        float travelled = 0f;
        float dt = Time.fixedDeltaTime;
        points.Add(position);

        // GroundCollide temporarily turns a landing surface into a trigger until takeoff.
        // Remember only initial contacts; after leaving, the same surface can be hit again.
        foreach (Collider2D source in colliders)
        {
            if (!source.enabled || source.isTrigger)
                continue;
            ContactFilter2D filter = CreateFilter(source);
            source.Overlap(filter, overlaps);
            foreach (Collider2D other in overlaps)
            {
                if (other.attachedRigidbody == body || Physics2D.GetIgnoreCollision(source, other))
                    continue;
                ContactKind kind = classify(other);
                if (other.isTrigger && IsGround(kind))
                    initialContacts[other] = Vector2.zero;
                else if (!other.isTrigger)
                {
                    ColliderDistance2D separation = source.Distance(other);
                    if (separation.isValid)
                        initialContacts[other] = -separation.normal;
                }
            }
        }

        int steps = Mathf.CeilToInt(Mathf.Max(0f, duration) / dt);
        for (int step = 0; step < steps && travelled < maxDistance; step++)
        {
            float stepDuration = Mathf.Min(dt, duration - step * dt);
            // Avoid an extra, almost zero-length step from floating-point rounding.
            if (stepDuration <= dt * 0.0001f)
                break;
            // Same ordering as FixedUpdate followed by the Physics2D integration.
            velocity.y -= gravity * dt;
            velocity /= 1f + Mathf.Max(0f, damping) * dt;
            velocity = Vector2.ClampMagnitude(velocity, Physics2D.maxTranslationSpeed);
            Vector2 delta = velocity * stepDuration;
            float distance = delta.magnitude;
            if (distance < 0.000001f)
                continue;

            float remaining = Mathf.Max(0f, maxDistance - travelled);
            if (distance > remaining)
            {
                delta *= remaining / distance;
                distance = remaining;
            }

            float nearest = float.PositiveInfinity;
            RaycastHit2D nearestHit = default;
            ContactKind nearestKind = ContactKind.Ignore;
            stillOverlapping.Clear();

            foreach (Collider2D source in colliders)
            {
                if (!source.enabled || source.isTrigger)
                    continue;
                source.Cast(position, angle, delta / distance, CreateFilter(source), hits, distance, true);
                foreach (RaycastHit2D hit in hits)
                {
                    Collider2D other = hit.collider;
                    if (other == null || other.attachedRigidbody == body || Physics2D.GetIgnoreCollision(source, other))
                        continue;

                    ContactKind kind = classify(other);
                    if (kind == ContactKind.Ignore)
                        continue;
                    if (hit.distance <= 0.0001f && initialContacts.TryGetValue(other, out Vector2 normal))
                    {
                        stillOverlapping.Add(other);
                        if ((other.isTrigger && IsGround(kind)) || Vector2.Dot(delta, normal) > 0f)
                            continue;
                    }

                    // Prefer a lethal contact when two surfaces touch at the same distance.
                    if (hit.distance < nearest || (Mathf.Abs(hit.distance - nearest) < 0.0001f && IsCrash(kind)))
                    {
                        nearest = hit.distance;
                        nearestHit = hit;
                        nearestKind = kind;
                    }
                }
            }

            exited.Clear();
            foreach (Collider2D other in initialContacts.Keys)
                if (!stillOverlapping.Contains(other))
                    exited.Add(other);
            foreach (Collider2D other in exited)
                initialContacts.Remove(other);

            Vector2 end = position + delta * (nearestKind == ContactKind.Ignore ? 1f : Mathf.Clamp01(nearest / distance));
            if (outsideBounds != null && outsideBounds(end))
            {
                Vector2 inside = position;
                Vector2 outside = end;
                for (int iteration = 0; iteration < 16; iteration++)
                {
                    Vector2 middle = (inside + outside) * 0.5f;
                    if (outsideBounds(middle)) outside = middle;
                    else inside = middle;
                }
                points.Add(inside);
                return new Result { crash = true, point = inside, normal = -delta.normalized };
            }

            points.Add(end);
            if (nearestKind != ContactKind.Ignore)
                return new Result { crash = IsCrash(nearestKind), point = nearestHit.point, normal = nearestHit.normal };

            travelled += distance;
            position = end;
        }

        return default;
    }

    private static ContactFilter2D CreateFilter(Collider2D collider)
    {
        ContactFilter2D filter = new ContactFilter2D();
        int mask = Physics2D.GetLayerCollisionMask(collider.gameObject.layer);
        mask |= collider.includeLayers.value;
        mask &= ~collider.excludeLayers.value;
        filter.SetLayerMask(mask);
        filter.useTriggers = true;
        return filter;
    }

    private static bool IsGround(ContactKind kind) => kind == ContactKind.Ground || kind == ContactKind.DangerousGround;
    private static bool IsCrash(ContactKind kind) => kind == ContactKind.Crash || kind == ContactKind.DangerousGround;
}
