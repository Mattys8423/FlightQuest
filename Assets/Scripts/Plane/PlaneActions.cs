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
    public int NumberOfLaunch;
    public int trajectoryPoints = 30;
    public int SkillNumber = 0;
    public LineRenderer lineRenderer;
    public GameObject impactMarkerPrefab;

    public float alpha = 45f;
    public float f2 = 0.1f;
    public float g = 9.81f;

    void Start()
    {
        SkillNumber = Main.GetPlane();
        Debug.Log(SkillNumber + "is your plane");
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.isKinematic = true;

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
                    Vector2 direction;
                    Vector2 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (Main.GetBoolInversed() == true)
                    {
                        direction = startPos - currentPos;
                    }
                    else
                    {
                        direction = currentPos - startPos;
                    }
                    Vector2 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    switch (FirstLaunch)
                    {
                        case false:
                            if (Vector2.Distance(endPos, startPos) < LimitDrag || endPos.y - startPos.y < LimitDrag)
                            {
                                lineRenderer.enabled = false;
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
                                lineRenderer.enabled = false;
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
                    Vector2 direction;
                    if (Main.GetBoolInversed() == true)
                    {
                        direction = startPos - endPos;
                    }
                    else
                    {
                        direction = endPos - startPos;
                    }
                    switch (FirstLaunch)
                    {                        
                        case false:
                            if (Vector2.Distance(endPos, startPos) < LimitDrag || endPos.y - startPos.y < LimitDrag) { }
                            else
                            {
                                StartCoroutine(script.LaunchPuff());
                                rb.isKinematic = false;
                                rb.constraints = RigidbodyConstraints2D.None;
                                isFlying = true;
                                rb.AddForce(direction * launchForce, ForceMode2D.Impulse);
                                this.GetComponent<Animator>().SetBool("IsFlying", true);

                                lineRenderer.enabled = false;
                                isDragging = false;
                                IsGrounded = false;
                                FirstLaunch = false;
                                NumberOfLaunch -= 1;
                                if (activeImpactMarker != null)
                                {
                                    activeImpactMarker.SetActive(false);
                                }
                            }
                            break;
                        case true:
                            if (Vector2.Distance(endPos, startPos) < LimitDrag) { }
                            else
                            {
                                StartCoroutine(script.LaunchPuff());
                                rb.isKinematic = false;
                                rb.constraints = RigidbodyConstraints2D.None;
                                isFlying = true;
                                rb.AddForce(direction * launchForce, ForceMode2D.Impulse);
                                this.GetComponent<Animator>().SetBool("IsFlying", true);

                                lineRenderer.enabled = false;
                                isDragging = false;
                                IsGrounded = false;
                                FirstLaunch = false;
                                NumberOfLaunch -= 1;
                                if (activeImpactMarker != null)
                                {
                                    activeImpactMarker.SetActive(false);
                                }
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

    void ShowTrajectory(Vector2 start, Vector2 velocity)
    {
        List<Vector3> points = new List<Vector3>();

        Vector2 pos = start;
        Vector2 vel = velocity;

        float detectionRadius = 0.45f;
        LayerMask obstacleLayer = LayerMask.GetMask("Obstacle");
        Vector2 previousPos = pos;

        float dt = 0.1f;
        List<float> liste_x = new List<float> { 0 };
        List<float> liste_y = new List<float> { 0 };

        float vx = vel.x;
        float vy = vel.y;

        float angleRad = alpha * Mathf.Deg2Rad;
        float v0 = velocity.magnitude;

        bool collisionDetected = false;
        int collisionIndex = -1; // ← on mémorise où la collision est arrivée

        // Calculer la trajectoire avec friction
        for (int i = 0; i < trajectoryPoints; i++)
        {
            points.Add(pos);

            Vector2 deltaPos = pos - previousPos;

            RaycastHit2D hit = Physics2D.CircleCast(previousPos, detectionRadius, deltaPos.normalized, deltaPos.magnitude, obstacleLayer);
            if (hit.collider != null)
            {
                Debug.Log("Collision détectée avec : " + hit.collider.name);

                collisionDetected = true;
                collisionIndex = i;

                Vector2 incomingDirection = deltaPos.normalized;
                Vector2 normal = hit.normal;
                float angleIncidence = Vector2.Angle(-incomingDirection, normal);

                Debug.Log("Angle d'incidence: " + angleIncidence + "°");

                if (activeImpactMarker == null)
                {
                    activeImpactMarker = Instantiate(impactMarkerPrefab, hit.point, Quaternion.identity);
                    activeImpactMarker.transform.rotation = Quaternion.LookRotation(Vector3.forward, hit.normal);
                    activeImpactMarker.SetActive(true);
                }
                else
                {
                    activeImpactMarker.transform.position = hit.point;
                    activeImpactMarker.transform.rotation = Quaternion.LookRotation(Vector3.forward, hit.normal);
                    activeImpactMarker.SetActive(true);
                }
            }

            vx += -f2 * vx * dt;
            vy += -(g + f2 * vy) * dt;
            previousPos = pos;
            pos.x += vx * dt;
            pos.y += vy * dt;

            liste_x.Add(pos.x);
            liste_y.Add(pos.y);
        }

        if (!collisionDetected && activeImpactMarker != null)
        {
            activeImpactMarker.SetActive(false);
        }

        // Dessiner la trajectoire
        lineRenderer.positionCount = points.Count > trajectoryPoints ? trajectoryPoints : points.Count;
        lineRenderer.SetPositions(points.ToArray());

        // Changer l'apparence selon collision
        Gradient gradient = new Gradient();

        if (collisionDetected && collisionIndex >= 0)
        {
            // Cas où il y a eu collision → rendre la suite transparente
            float collisionPointTime = (float)collisionIndex / (float)trajectoryPoints;

            gradient.SetKeys(
                new GradientColorKey[] {
                new GradientColorKey(Color.white, 0.0f),
                new GradientColorKey(Color.white, collisionPointTime),
                new GradientColorKey(new Color(1f, 1f, 1f, 0f), 1.0f) // ← Transparence après collision
                },
                new GradientAlphaKey[] {
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(1.0f, collisionPointTime),
                new GradientAlphaKey(0.0f, 1.0f)
                }
            );
        }
        else
        {
            // Pas de collision → ligne classique
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
        }

        lineRenderer.colorGradient = gradient;

        Vector2 finalVelocity = new Vector2(vx, vy);
        float finalSpeed = finalVelocity.magnitude;

        float maxSpeed = 15f;
        lineRenderer.material.SetFloat("_TilingAmount", finalSpeed * 1.2f);
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
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        this.GetComponent<Animator>().SetBool("IsFlying", animation);
        isFlying = false;
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
