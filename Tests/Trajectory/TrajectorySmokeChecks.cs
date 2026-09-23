// Run in an isolated Unity 6 project containing this file under Assets/Editor
// and Assets/Scripts/Plane/PlaneTrajectory.cs. No test package is required.
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class TrajectorySmokeChecks
{
    private static readonly List<GameObject> objects = new List<GameObject>();
    private static readonly List<Vector3> points = new List<Vector3>();
    private static int failures;

    public static void Run()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;
        Time.fixedDeltaTime = 0.02f;
        Check("free flight matches Unity physics", FreeFlight);
        Check("arc length limited even at high speed", DistanceLimit);
        Check("no cross beyond distance limit", HiddenHazard);
        Check("real contour catches wing contact", WingContact);
        Check("polygon empty corner is not a collision", EmptyCorner);
        Check("safe landing stops before later danger", Landing);
        Check("takeoff from touching solid ground", () => Takeoff(false, 0.5f));
        Check("takeoff from overlapping solid ground", () => Takeoff(false, 0.49f));
        Check("trigger ground rearmed after takeoff", () => Takeoff(true, 0.49f));
        Check("ignored collectibles do not hide hazard", Collectibles);
        Check("movement changing trigger stops preview", StopTrigger);
        Check("ignored collision pairs respected", IgnoredPair);
        Check("initial lethal overlap not discarded", InitialHazard);
        Check("camera boundary clips preview", CameraBoundary);
        Check("mirrored offset collider follows leftward launch", Mirrored);
        Debug.Log("TRAJECTORY_CHECKS_COMPLETE failures=" + failures);
        EditorApplication.Exit(failures == 0 ? 0 : 1);
    }

    private static void Check(string name, Action test)
    {
        try { test(); Debug.Log("PASS: " + name); }
        catch (Exception e) { failures++; Debug.LogError("FAIL: " + name + " " + e); }
        finally
        {
            foreach (GameObject obj in objects) UnityEngine.Object.DestroyImmediate(obj);
            objects.Clear();
            Physics2D.SyncTransforms();
        }
    }

    private static GameObject Make(string name, Vector2 position)
    {
        var obj = new GameObject(name);
        obj.transform.position = position;
        objects.Add(obj);
        return obj;
    }

    private static Rigidbody2D Plane(Vector2 position)
    {
        var obj = Make("plane", position);
        var body = obj.AddComponent<Rigidbody2D>();
        body.gravityScale = 0f;
        body.linearDamping = 0f;
        body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        obj.AddComponent<BoxCollider2D>().size = Vector2.one;
        return body;
    }

    private static BoxCollider2D Box(string name, Vector2 position, Vector2 size, bool trigger = false)
    {
        var collider = Make(name, position).AddComponent<BoxCollider2D>();
        collider.size = size;
        collider.isTrigger = trigger;
        return collider;
    }

    private static PlaneTrajectory.ContactKind Kind(Collider2D collider)
    {
        switch (collider.name)
        {
            case "danger": return PlaneTrajectory.ContactKind.Crash;
            case "ground": return PlaneTrajectory.ContactKind.Ground;
            case "stop": return PlaneTrajectory.ContactKind.Stop;
            default: return PlaneTrajectory.ContactKind.Ignore;
        }
    }

    private static PlaneTrajectory.Result Predict(Rigidbody2D body, Vector2 impulse,
        float gravity = 0f, float damping = 0f, float duration = 1.5f, float limit = 6f)
    {
        Physics2D.SyncTransforms();
        return new PlaneTrajectory().Predict(body, impulse, gravity, damping, duration, limit, points, Kind);
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
    }

    private static void FreeFlight()
    {
        var body = Plane(Vector2.zero);
        body.mass = 2f;
        body.linearDamping = 0.7f;
        Vector2 impulse = new Vector2(14f, 19f);
        Predict(body, impulse, 9.81f, 0.7f, 1.5f, 100f);
        Vector3[] expected = points.ToArray();
        body.AddForce(impulse, ForceMode2D.Impulse);
        float maxError = 0f;
        for (int i = 1; i < expected.Length; i++)
        {
            body.linearVelocity += Vector2.down * 9.81f * Time.fixedDeltaTime;
            Physics2D.Simulate(Time.fixedDeltaTime);
            maxError = Mathf.Max(maxError, Vector2.Distance(body.position, expected[i]));
        }
        Require(maxError < 0.002f, "max position error=" + maxError);
    }

    private static void DistanceLimit()
    {
        Predict(Plane(Vector2.zero), new Vector2(50f, 30f), 9.81f);
        float length = 0f;
        for (int i = 1; i < points.Count; i++) length += Vector3.Distance(points[i - 1], points[i]);
        Require(Mathf.Abs(length - 6f) < 0.0001f, "length=" + length);
    }

    private static void HiddenHazard()
    {
        var body = Plane(Vector2.zero);
        Box("danger", new Vector2(8f, 0f), Vector2.one);
        Require(!Predict(body, Vector2.right * 20f).crash, "danger beyond horizon was revealed");
        Require(Mathf.Abs(points[points.Count - 1].x - 6f) < 0.001f, "end point was not clipped");
    }

    private static void WingContact()
    {
        var body = Plane(Vector2.zero);
        var danger = Box("danger", new Vector2(2f, 0.47f), Vector2.one * 0.06f);
        var result = Predict(body, Vector2.right * 10f);
        Require(result.crash, "outer wing contact missed on Default layer");
        float predictedX = points[points.Count - 1].x;
        body.AddForce(Vector2.right * 10f, ForceMode2D.Impulse);
        bool touched = false;
        for (int i = 0; i < 30; i++)
        {
            Physics2D.Simulate(Time.fixedDeltaTime);
            if (body.IsTouching(danger)) { touched = true; break; }
        }
        Require(touched, "real physics did not hit the predicted obstacle");
        Require(Mathf.Abs(body.position.x - predictedX) < 0.1f, "predicted and real impact differ");
    }

    private static void EmptyCorner()
    {
        var body = Plane(Vector2.zero);
        UnityEngine.Object.DestroyImmediate(body.GetComponent<BoxCollider2D>());
        body.gameObject.AddComponent<PolygonCollider2D>().points = new[] {
            new Vector2(-0.5f, -0.5f), new Vector2(0.5f, 0f), new Vector2(-0.5f, 0.5f) };
        Box("danger", new Vector2(1.8f, 0.43f), Vector2.one * 0.05f);
        Require(!Predict(body, Vector2.right * 10f, limit: 1.5f).crash, "bounding rectangle used instead of triangle");
    }

    private static void Landing()
    {
        var body = Plane(new Vector2(0f, 3f));
        Box("ground", new Vector2(0f, -0.5f), new Vector2(20f, 1f));
        Box("danger", new Vector2(0f, -2f), Vector2.one);
        Require(!Predict(body, Vector2.down * 10f).crash, "safe landing reported as crash");
        Require(points[points.Count - 1].y > 0.45f, "preview went through landing surface");
    }

    private static void Takeoff(bool trigger, float height)
    {
        var body = Plane(new Vector2(0f, height));
        Box("ground", new Vector2(0f, -0.5f), new Vector2(20f, 1f), trigger);
        Predict(body, new Vector2(2f, 5f), 9.81f, duration: 2f, limit: 20f);
        Require(points.Count > 20, "preview stopped at initial contact, count=" + points.Count);
        Require(points[points.Count - 1].y > 0.45f, "ground not rearmed on return");
        Require(points[points.Count - 1].y < 0.6f, "did not land on return");
    }

    private static void Collectibles()
    {
        var body = Plane(Vector2.zero);
        for (int i = 0; i < 25; i++) Box("coin", new Vector2(1f + i * 0.03f, 0f), Vector2.one * 0.05f, true);
        Box("danger", new Vector2(3f, 0f), Vector2.one * 0.1f);
        Require(Predict(body, Vector2.right * 20f).crash, "trigger results hid the obstacle");
    }

    private static void StopTrigger()
    {
        var body = Plane(Vector2.zero);
        Box("stop", new Vector2(1f, 0f), Vector2.one * 0.1f, true);
        Box("danger", new Vector2(3f, 0f), Vector2.one);
        Require(!Predict(body, Vector2.right * 10f).crash, "preview continued beyond booster");
        Require(points[points.Count - 1].x < 1f, "stop trigger missed");
    }

    private static void IgnoredPair()
    {
        var body = Plane(Vector2.zero);
        var danger = Box("danger", new Vector2(2f, 0f), Vector2.one);
        Physics2D.IgnoreCollision(body.GetComponent<Collider2D>(), danger);
        Require(!Predict(body, Vector2.right * 10f).crash, "ignored collision was detected");
    }

    private static void InitialHazard()
    {
        var body = Plane(Vector2.zero);
        Box("danger", new Vector2(0.4f, 0f), Vector2.one);
        Require(Predict(body, Vector2.right * 10f).crash, "lethal initial overlap discarded");
    }

    private static void CameraBoundary()
    {
        var body = Plane(Vector2.zero);
        Physics2D.SyncTransforms();
        var result = new PlaneTrajectory().Predict(body, Vector2.right * 20f, 0f, 0f, 1.5f, 6f,
            points, Kind, p => p.x > 2f);
        Require(result.crash && Mathf.Abs(result.point.x - 2f) < 0.001f, "boundary not marked");
    }

    private static void Mirrored()
    {
        var body = Plane(Vector2.zero);
        body.GetComponent<BoxCollider2D>().offset = new Vector2(0.6f, 0f);
        body.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        Box("danger", new Vector2(-2f, 0f), Vector2.one * 0.1f);
        Require(Predict(body, Vector2.left * 10f).crash, "mirrored obstacle missed");
        Require(Mathf.Abs(points[points.Count - 1].x + 0.85f) < 0.08f, "mirrored offset not used");
        Require(body.position == Vector2.zero, "prediction moved real plane");
    }
}
