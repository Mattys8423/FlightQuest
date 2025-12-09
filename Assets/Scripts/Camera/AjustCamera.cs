using UnityEngine;

public class AjustCamera : MonoBehaviour
{
    private Camera cam;
    public float baseWidth = 12f; // largeur de référence de ton niveau en units

    void Start()
    {
        cam = GetComponent<Camera>();
        float targetRatio = baseWidth / cam.aspect;
        cam.orthographicSize = targetRatio;
    }
}
