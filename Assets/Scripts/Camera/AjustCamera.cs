using UnityEngine;

public class AjustCamera : MonoBehaviour
{
    private Camera cam;
    public float baseWidth = 12f; // largeur de référence de ton niveau en units

    void Start()
    {
        cam = GetComponent<Camera>();
        float targetRatio = baseWidth / cam.aspect;

        if (Screen.height > 1900)
        {
            cam.orthographicSize = targetRatio - 1;
        }
        else if (Screen.height < 1000)
        {
            cam.orthographicSize = targetRatio + 1;
        }
        else
        {
            cam.orthographicSize = targetRatio;
        }
    }
}
