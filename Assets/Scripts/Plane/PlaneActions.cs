using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneActions : MonoBehaviour
{
    [SerializeField] SaveStars Main;

    private Vector2 startPos;
    private float LimitDrag = 2f;
    private bool isDragging = false;
    private bool isFlying = false;
    private bool Skill = false;
    private bool IsGrounded = true;
    private bool FirstLaunch= true;
    private Rigidbody2D rb;
    private GameObject activeImpactMarker = null;
    [SerializeField] private Puff script;

    public float launchForce = 2f;
    [SerializeField, Min(0.1f)] private float maxDragDistance = 8f;
    public int NumberOfLaunch;
    public int trajectoryPoints = 20;
    [SerializeField, Min(0.5f)] private float trajectoryDuration = 1.5f;
    [SerializeField, Range(0.1f, 1f)] private float predictionCollisionScale = 0.65f;
    public int SkillNumber = 0;
    public LineRenderer lineRenderer;
    public GameObject impactMarkerPrefab;

    public float alpha = 45f;
    public float f2 = 0.1f;
    public float g = 9.81f;

    void Start()
    {
        SkillNumber = Main.GetPlane();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearDamping = f2;
        rb.bodyType = RigidbodyType2D.Kinematic;

        lineRenderer.positionCount = trajectoryPoints;
    }

    void Update()
    {
        switch (NumberOfLaunch)
        {
            case > 0:
                if (Input.GetMouseButtonDown(0) && IsGrounded)
                {
                    StopPlane(true);
                    startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    isDragging = true;
                    script.PlacePuff();
                }
                else if (Input.GetMouseButton(0) && isDragging)
                {
                    Vector2 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 direction = GetLimitedLaunchDirection(currentPos);
                    Vector2 endPos = currentPos;

                    switch (FirstLaunch)
                    {
                        case false:
                            if (Vector2.Distance(endPos, startPos) < LimitDrag || endPos.y - startPos.y < LimitDrag)
                            {
                                HideTrajectoryPreview();
                            }
                            else
                            {
                                lineRenderer.enabled = true;
                                ShowTrajectory(transform.position, direction * launchForce);
                            }
                            break;
                        case true:
                            if (Vector2.Distance(endPos, startPos) < LimitDrag)
                            {
                                HideTrajectoryPreview();
                            }
                            else
                            {
                                lineRenderer.enabled = true;
                                ShowTrajectory(transform.position, direction * launchForce);
                            }
                            break;
                    }
                    if (Main.GetBoolInversed() == false)
                    {
                        if (endPos.x - startPos.x >= 0)
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        else
                        {
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                    }
                    else
                    {
                        if (startPos.x - endPos.x >= 0)
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        else
                        {
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                    }
                }
                else if (Input.GetMouseButtonUp(0) && isDragging)
                {                   
                    Vector2 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 direction = GetLimitedLaunchDirection(endPos);
                    switch (FirstLaunch)
                    {                        
                        case false:
                            if (Vector2.Distance(endPos, startPos) < LimitDrag || endPos.y - startPos.y < LimitDrag)
                            {
                                isDragging = false;
                                HideTrajectoryPreview();
                            }
                            else
                            {
                                StartCoroutine(script.LaunchPuff());
                                rb.bodyType = RigidbodyType2D.Dynamic;
                                rb.constraints = RigidbodyConstraints2D.None;
                                isFlying = true;
                                rb.AddForce(direction * launchForce, ForceMode2D.Impulse);
                                this.GetComponent<Animator>().SetBool("IsFlying", true);

                                HideTrajectoryPreview();
                                isDragging = false;
                                IsGrounded = false;
                                FirstLaunch = false;
                                NumberOfLaunch -= 1;
                            }
                            break;
                        case true:
                            if (Vector2.Distance(endPos, startPos) < LimitDrag)
                            {
                                isDragging = false;
                                HideTrajectoryPreview();
                            }
                            else
                            {
                                StartCoroutine(script.LaunchPuff());
                                rb.bodyType = RigidbodyType2D.Dynamic;
                                rb.constraints = RigidbodyConstraints2D.None;
                                isFlying = true;
                                rb.AddForce(direction * launchForce, ForceMode2D.Impulse);
                                this.GetComponent<Animator>().SetBool("IsFlying", true);

                                HideTrajectoryPreview();
                                isDragging = false;
                                IsGrounded = false;
                                FirstLaunch = false;
                                NumberOfLaunch -= 1;
                            }
                            break;
                    }
                }
                break;
            case <= 0:
                break;
        }
    }

    void FixedUpdate()
    {
        if (isFlying)
        {
            rb.linearVelocity += new Vector2(0, -g * Time.fixedDeltaTime);
            rb.linearDamping = f2;
            if (Input.GetMouseButton(0) && !Skill)
            {
                SpecialSkill();
                Skill = true;
            }
        }
    }

    private Vector2 GetLimitedLaunchDirection(Vector2 pointerWorldPosition)
    {
        Vector2 rawDirection = Main.GetBoolInversed()
            ? startPos - pointerWorldPosition
            : pointerWorldPosition - startPos;

        return Vector2.ClampMagnitude(rawDirection, maxDragDistance);
    }

    void ShowTrajectory(Vector2 start, Vector2 impulse)
    {
        List<Vector3> points = new List<Vector3>();

        Vector2 pos = start;
        Vector2 vel = rb.linearVelocity + impulse / Mathf.Max(rb.mass, Mathf.Epsilon);

        Collider2D planeCollider = GetComponent<Collider2D>();
        // La prévision est volontairement un peu plus étroite que l'avion :
        // elle indique une zone de risque, sans révéler trop facilement chaque crash.
        Vector2 planeSize = planeCollider != null ? planeCollider.bounds.size : Vector2.one * 0.7f;
        Vector2 castSize = planeSize * predictionCollisionScale;
        LayerMask obstacleLayer = LayerMask.GetMask("Obstacle");
        ContactFilter2D obstacleFilter = new ContactFilter2D();
        obstacleFilter.SetLayerMask(obstacleLayer);
        obstacleFilter.useTriggers = false;

        float dt = Time.fixedDeltaTime;
        int simulationSteps = Mathf.CeilToInt(trajectoryDuration / dt);
        int renderedPointCount = Mathf.Max(2, trajectoryPoints);
        float sampleInterval = trajectoryDuration / (renderedPointCount - 1);
        float nextSampleTime = sampleInterval;
        RaycastHit2D[] hits = new RaycastHit2D[16];

        bool collisionDetected = false;
        points.Add(pos);

        for (int i = 1; i <= simulationSteps; i++)
        {
            Vector2 previousPos = pos;

            // FixedUpdate applique cette gravité manuelle avant que Rigidbody2D
            // n'applique son amortissement et intègre la nouvelle position.
            vel.y -= g * dt;
            vel /= 1f + f2 * dt;
            pos += vel * dt;

            Vector2 deltaPos = pos - previousPos;
            RaycastHit2D hit;
            if (TryGetFirstImpact(previousPos, deltaPos, castSize, obstacleFilter, hits, out hit))
            {
                collisionDetected = true;
                points.Add(hit.centroid);
                ShowImpactMarker(hit);
                break;
            }

            float elapsedTime = i * dt;
            if (elapsedTime + dt * 0.5f >= nextSampleTime)
            {
                points.Add(pos);
                nextSampleTime += sampleInterval;
            }
        }

        if (!collisionDetected && activeImpactMarker != null)
        {
            activeImpactMarker.SetActive(false);
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
            new GradientColorKey(Color.white, 0.0f),
            new GradientColorKey(Color.white, 1.0f)
            },
            new GradientAlphaKey[] {
            new GradientAlphaKey(1.0f, 0.0f),
            new GradientAlphaKey(1.0f, 1.0f)
            }
        );

        lineRenderer.colorGradient = gradient;

        lineRenderer.material.SetFloat("_TilingAmount", vel.magnitude * 1.2f);
    }

    private bool TryGetFirstImpact(
        Vector2 origin,
        Vector2 delta,
        Vector2 castSize,
        ContactFilter2D obstacleFilter,
        RaycastHit2D[] hits,
        out RaycastHit2D firstHit)
    {
        firstHit = default;
        float distance = delta.magnitude;
        if (distance <= Mathf.Epsilon)
        {
            return false;
        }

        int hitCount = Physics2D.BoxCast(
            origin,
            castSize,
            rb.rotation,
            delta / distance,
            obstacleFilter,
            hits,
            distance);

        float nearestDistance = float.PositiveInfinity;
        bool found = false;

        for (int i = 0; i < hitCount; i++)
        {
            // Un avion posé touche déjà le sol. Les impacts à distance
            // nulle correspondent à ce contact initial, pas au futur crash.
            if (hits[i].collider == null || hits[i].distance <= 0.001f || hits[i].distance >= nearestDistance)
            {
                continue;
            }

            nearestDistance = hits[i].distance;
            firstHit = hits[i];
            found = true;
        }

        return found;
    }

    private void ShowImpactMarker(RaycastHit2D hit)
    {
        if (activeImpactMarker == null)
        {
            activeImpactMarker = Instantiate(impactMarkerPrefab, hit.point, Quaternion.identity);
        }

        activeImpactMarker.transform.position = hit.point;
        activeImpactMarker.transform.rotation = Quaternion.LookRotation(Vector3.forward, hit.normal);
        activeImpactMarker.SetActive(true);
    }

    private void HideTrajectoryPreview()
    {
        lineRenderer.enabled = false;
        if (activeImpactMarker != null)
        {
            activeImpactMarker.SetActive(false);
        }
    }



    private void SpecialSkill()
    {
        switch (SkillNumber)
        {
            case 0:
                rb.linearVelocity += new Vector2(0, 10);
                break;

            case 1:
                rb.linearVelocity += new Vector2(10, 0);
                break;

            case 2:
                StartCoroutine(DoLooping());
                break;
            case 3:
                StartCoroutine(NoGravity());
                break;
        }
    }

    private IEnumerator DoLooping()
    {
        float duration = 0.6f;
        float radius = -2f;
        float angle = 0f;

        Vector2 originalVelocity = rb.linearVelocity;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;

        Vector2 center = rb.position + new Vector2(0, -radius);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            angle = Mathf.Lerp(0, -360, t);
            float rad = angle * Mathf.Deg2Rad;

            Vector2 offset = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)) * radius;
            rb.MovePosition(center + offset);
            rb.MoveRotation(-angle);

            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = originalVelocity;
        rb.gravityScale = originalGravity;

        rb.MoveRotation(0f);
    }

    private IEnumerator NoGravity()
    {
        float SavedGravity = g;
        g = 0;
        yield return new WaitForSeconds(2f);
        g = SavedGravity;
    }


    //--------------------------------------------------------------------------//

    public void StopPlane(bool animation)
    {
        StopAllCoroutines();
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        this.GetComponent<Animator>().SetBool("IsFlying", animation);
        isFlying = false;
    }

    public void DisablePlaneWithoutExplosion()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetBool("IsFlying", false);
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.enabled = false;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        IsGrounded = false;
        isFlying = false;
        isDragging = false;
    }

    public void SetGrounded(bool grounded) { IsGrounded = grounded; }

    public bool GetDJ() { return Skill; }

    public bool GetFL() { return FirstLaunch; }

    public bool GetIsFlying() {  return isFlying; }

    public void AddForceY(float value)
    {
        rb.AddForce(new Vector2(0, value), ForceMode2D.Impulse);
    }

    public void AddForceX(float value)
    {
        rb.AddForce(new Vector2(value, 0), ForceMode2D.Impulse);
    }

    public void changeLimit(float value)
    {
        LimitDrag = value;
    }
}
