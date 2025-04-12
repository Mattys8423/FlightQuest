using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneActions : MonoBehaviour
{
    [SerializeField] SaveStars Main;

    private Vector2 startPos;
    private bool isDragging = false;
    private bool isFlying = false;
    private bool Skill = false;
    private bool IsGrounded = true;
    private bool FirstLaunch= true;
    private Rigidbody2D rb;
    [SerializeField] private Puff script;

    public float launchForce = 2f;
    public int NumberOfLaunch;
    public int trajectoryPoints = 30;
    public int SkillNumber = 0;
    public LineRenderer lineRenderer;

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
                    Vector2 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 direction = currentPos - startPos;
                    Vector2 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    switch (FirstLaunch)
                    {
                        case false:
                            if (Vector2.Distance(endPos, startPos) < 2f || endPos.y - startPos.y < 2)
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
                            if (Vector2.Distance(endPos, startPos) < 2f)
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
                }
                else if (Input.GetMouseButtonUp(0) && isDragging)
                {                   
                    Vector2 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Debug.Log(IsGrounded);
                    switch (FirstLaunch)
                    {
                        case false:
                            if (Vector2.Distance(endPos, startPos) < 2f || endPos.y - startPos.y < 2) { }
                            else
                            {
                                Vector2 direction = endPos - startPos;

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
                            }
                            break;
                        case true:
                            if (Vector2.Distance(endPos, startPos) < 2f) { }
                            else
                            {
                                Vector2 direction = endPos - startPos;

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

        float dt = 0.1f;
        List<float> liste_x = new List<float> { 0 };
        List<float> liste_y = new List<float> { 0 };

        float vx = vel.x;
        float vy = vel.y;

        float angleRad = alpha * Mathf.Deg2Rad;
        float v0 = velocity.magnitude;

        // Calculer la trajectoire avec friction
        for (int i = 0; i < trajectoryPoints; i++)
        {
            points.Add(pos);
            vx += -f2 * vx * dt; // Mise à jour de la vitesse horizontale avec friction
            vy += -(g + f2 * vy) * dt; // Mise à jour de la vitesse verticale avec friction
            pos.x += vx * dt;  // Mise à jour de la position horizontale
            pos.y += vy * dt;  // Mise à jour de la position verticale

            // Ajouter les positions à la liste
            liste_x.Add(pos.x);
            liste_y.Add(pos.y);
        }

        lineRenderer.positionCount = points.Count > trajectoryPoints ? trajectoryPoints : points.Count;
        lineRenderer.SetPositions(points.ToArray());

        Vector2 finalVelocity = new Vector2(vx, vy);
        float finalSpeed = finalVelocity.magnitude;

        float maxSpeed = 15f;
        float t = Mathf.Clamp01(finalSpeed / maxSpeed);
        float tReverse = Mathf.Clamp01(maxSpeed / finalSpeed);

        Color coldColor = Color.white;
        Color hotColor = new Color(255, 0, 0);
        Color finalColor = Color.Lerp(coldColor, hotColor, t);

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
            new GradientColorKey(coldColor, 0.0f),
            new GradientColorKey(finalColor, tReverse)
            },
            new GradientAlphaKey[] {
            new GradientAlphaKey(1.0f, 0.0f),
            new GradientAlphaKey(t, 1.0f)
            }
        );

        lineRenderer.colorGradient = gradient;
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

    public bool GetIsFlying() {  return isFlying; }

    public void AddForceY(float value)
    {
        rb.AddForce(new Vector2(0, value), ForceMode2D.Impulse);
    }
}
