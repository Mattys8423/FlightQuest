using System.Collections.Generic;
using UnityEngine;

public class TutorialTrajectory : MonoBehaviour
{
    [SerializeField] PlaneActions script;
    public LineRenderer lineRenderer;
    public int trajectoryPoints = 30;
    public float dt = 0.1f;
    public Vector2 start = Vector2.zero;
    public Vector2 initialVelocity = new Vector2(5f, 5f);
    public float g = 9.81f;
    public float f2 = 0.2f; // friction

    void Start()
    {
        List<Vector3> points = new List<Vector3>();

        Vector2 pos = start;
        float vx = initialVelocity.x;
        float vy = initialVelocity.y;

        for (int i = 0; i < trajectoryPoints; i++)
        {
            points.Add(pos);

            vx += -f2 * vx * dt;
            vy += -(g + f2 * vy) * dt;

            pos.x += vx * dt;
            pos.y += vy * dt;
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    private void Update()
    {
        if (script.GetFL() == false)
        {
            lineRenderer.enabled = false;
        }
    }
}

