using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneActions : MonoBehaviour
{
    private Vector2 startPos;
    private bool isDragging = false;
    private bool isFlying = false;
    private bool DoubleJumpUse = false;
    private bool IsGrounded = true;
    private Rigidbody2D rb;
    [SerializeField] private Puff script;

    public float launchForce = 10f;
    public int NumberOfLaunch;
    public int trajectoryPoints = 30;
    public LineRenderer lineRenderer;

    public float alpha = 45f; // Angle de lancement en degrés
    public float f2 = 0.1f; // Coefficient de friction
    public float g = 9.81f; // Gravité

    void Start()
    {
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
                    Vector2 direction = startPos - currentPos;

                    ShowTrajectory(transform.position, direction * launchForce);
                }
                else if (Input.GetMouseButtonUp(0) && isDragging)
                {
                    Vector2 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 direction = startPos - endPos;

                    StartCoroutine(script.LaunchPuff());
                    rb.isKinematic = false;
                    isFlying = true;
                    rb.AddForce(direction * launchForce, ForceMode2D.Impulse);
                    this.GetComponent<Animator>().SetBool("IsFlying", true);

                    lineRenderer.enabled = false;
                    isDragging = false;
                    IsGrounded = false;
                    NumberOfLaunch -= 1;
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
            if (Input.GetMouseButton(0) && !DoubleJumpUse)
            {
                rb.linearVelocity += new Vector2(0, 10);
                DoubleJumpUse = true;
            }
        }
    }

    void ShowTrajectory(Vector2 start, Vector2 velocity)
    {
        lineRenderer.enabled = true;
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
    }

//--------------------------------------------------------------------------//

    public void StopPlane(bool animation)
    {
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        this.GetComponent<Animator>().SetBool("IsFlying", animation);
        isFlying = false;
    }

    public void SetGrounded(bool grounded) { IsGrounded = grounded; }
}
