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
    [Tooltip("Longueur maximale de la trajectoire affichée, en unités du monde.")]
    [SerializeField, Min(0.1f)] private float maxTrajectoryDistance = 6f;
    public int SkillNumber = 0;
    public LineRenderer lineRenderer;
    public GameObject impactMarkerPrefab;

    public float alpha = 45f;
    public float f2 = 0.1f;
    public float g = 9.81f;

    private readonly PlaneTrajectory trajectory = new PlaneTrajectory();
    private readonly List<Vector3> predictedPoints = new List<Vector3>();
    private CameraBounds cameraBounds;

    void Start()
    {
        SkillNumber = Main.GetPlane();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearDamping = f2;
        rb.bodyType = RigidbodyType2D.Kinematic;
        // Keep fast flights from passing through thin hazards between physics steps.
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        cameraBounds = Camera.main != null ? Camera.main.GetComponent<CameraBounds>() : null;

        lineRenderer.useWorldSpace = true;
        HideTrajectoryPreview();
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
                    FaceLaunchDirection(direction);

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
                                ShowTrajectory(direction * launchForce);
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
                                ShowTrajectory(direction * launchForce);
                            }
                            break;
                    }
                }
                else if (Input.GetMouseButtonUp(0) && isDragging)
                {                   
                    Vector2 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 direction = GetLimitedLaunchDirection(endPos);
                    FaceLaunchDirection(direction);
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

    private void FaceLaunchDirection(Vector2 direction)
    {
        transform.rotation = Quaternion.Euler(0f, direction.x >= 0f ? 0f : 180f, 0f);
    }

    void ShowTrajectory(Vector2 impulse)
    {
        // Facing may have changed this frame; queries must use the same mirrored polygon as launch.
        Physics2D.SyncTransforms();
        PlaneTrajectory.Result result = trajectory.Predict(rb, impulse, g, f2,
            trajectoryDuration, maxTrajectoryDistance, predictedPoints, ClassifyTrajectoryContact,
            IsOutsideFlightBounds);

        int count = Mathf.Min(predictedPoints.Count, Mathf.Max(2, trajectoryPoints));
        lineRenderer.positionCount = count;
        for (int i = 0; i < count; i++)
        {
            int index = count == 1 ? 0 : Mathf.RoundToInt(i * (predictedPoints.Count - 1f) / (count - 1));
            lineRenderer.SetPosition(i, predictedPoints[index]);
        }

        if (result.crash)
            ShowImpactMarker(result.point, result.normal);
        else if (activeImpactMarker != null)
            activeImpactMarker.SetActive(false);

        lineRenderer.material.SetFloat("_TilingAmount", (impulse / rb.mass).magnitude * 1.2f);
    }

    private PlaneTrajectory.ContactKind ClassifyTrajectoryContact(Collider2D other)
    {
        // A cross means an actual crash, not merely touching a collider.
        if (!other.isTrigger && other.TryGetComponent<Poison>(out var poison) && poison.isActiveAndEnabled)
            return PlaneTrajectory.ContactKind.Crash;
        if (other.TryGetComponent<GroundCollide>(out var ground) && ground.isActiveAndEnabled)
            return GroundCollide.IsCrashAngle(transform.eulerAngles.z)
                ? PlaneTrajectory.ContactKind.DangerousGround : PlaneTrajectory.ContactKind.Ground;
        if (other.TryGetComponent<GroundCollideTuto>(out var tutorialGround) && tutorialGround.isActiveAndEnabled)
            return PlaneTrajectory.ContactKind.Ground;
        if (!other.isTrigger)
            return PlaneTrajectory.ContactKind.Stop;

        // These triggers alter/stop the real flight. Do not show an invented path beyond them.
        if (other.GetComponent<Booster>() != null || other.GetComponent<StopZone>() != null ||
            other.GetComponent<StopZone1>() != null || other.GetComponent<DetectLanding>() != null ||
            other.GetComponent<DetectLandingHangar>() != null || other.GetComponent<DetectOutSpace>() != null ||
            other.GetComponent<GroundSetOutSpace>() != null || other.GetComponent<Pyramid>() != null)
            return PlaneTrajectory.ContactKind.Stop;
        return PlaneTrajectory.ContactKind.Ignore;
    }

    private bool IsOutsideFlightBounds(Vector2 position)
    {
        return cameraBounds != null && cameraBounds.isActiveAndEnabled &&
            cameraBounds.IsOutsideBounds(new Vector3(position.x, position.y, transform.position.z));
    }

    private void ShowImpactMarker(Vector2 point, Vector2 normal)
    {
        if (impactMarkerPrefab == null)
            return;
        if (activeImpactMarker == null)
        {
            activeImpactMarker = Instantiate(impactMarkerPrefab, point, Quaternion.identity);
        }

        activeImpactMarker.transform.position = point;
        activeImpactMarker.transform.rotation = Quaternion.LookRotation(Vector3.forward, normal);
        activeImpactMarker.SetActive(true);
    }

    private void OnDestroy()
    {
        if (activeImpactMarker != null)
            Destroy(activeImpactMarker);
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
