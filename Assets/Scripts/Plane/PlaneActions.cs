using System.Collections.Generic;
using UnityEngine;

public class PlaneActions : MonoBehaviour
{
    public float launchForce = 10f;
    public int NumberOfLaunch;
    public int trajectoryPoints = 30;
    public LineRenderer lineRenderer;

    private Vector2 startPos;
    private bool isDragging = false;
    private Rigidbody2D rb;

    public float alpha = 45f; // Angle de lancement en degrés
    public float f2 = 0.1f; // Coefficient de friction
    public float g = 9.81f; // Gravité

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1;
        rb.isKinematic = true;

        lineRenderer.positionCount = trajectoryPoints;
    }

    void Update()
    {
        switch (NumberOfLaunch)
        {
            case > 0:
                if (Input.GetMouseButtonDown(0))
                {
                    startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    isDragging = true;
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

                    rb.isKinematic = false;
                    rb.linearVelocity = direction * launchForce;

                    lineRenderer.enabled = false;
                    isDragging = false;
                    NumberOfLaunch -= 1;
                }
                break;
            case <= 0:
                break;
        }
    }

    // Fonction pour afficher la trajectoire avec friction
    void ShowTrajectory(Vector2 start, Vector2 velocity)
    {
        lineRenderer.enabled = true;
        List<Vector3> points = new List<Vector3>();

        Vector2 pos = start;
        Vector2 vel = velocity;

        float dt = 0.1f;  // Intervalle de temps
        List<float> liste_x = new List<float> { 0 };
        List<float> liste_y = new List<float> { 0 };

        float vx = vel.x;
        float vy = vel.y;

        // Convertir alpha en radians
        float angleRad = alpha * Mathf.Deg2Rad;

        // Initialiser la vitesse initiale en fonction de l'angle
        float v0 = velocity.magnitude; // Vitesse initiale

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
}
