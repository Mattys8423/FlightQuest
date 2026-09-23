using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CameraBounds : MonoBehaviour
{
    [Header("References (assign in Inspector)")]
    [SerializeField] private GameObject planeGO;
    [SerializeField] private GameObject explosion;
    [SerializeField] private EndLevelMenu script3;
    [SerializeField] private SaveStars script4;

    [Header("Settings")]
    [SerializeField] private float margin = 0f;

    [SerializeField] private bool checkTop = true;
    [SerializeField] private bool checkBottom = true;
    [SerializeField] private bool checkLeft = true;
    [SerializeField] private bool checkRight = true;

    private string levelName;
    private bool defeated = false;
    private PlaneActions planeScript;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        planeScript = planeGO.GetComponent<PlaneActions>();
        levelName = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        if (defeated) return;

        if (IsOutsideBounds(planeGO.transform.position))
        {
            TriggerDefeat();
        }
    }

    public bool IsOutsideBounds(Vector3 worldPosition)
    {
        if (cam == null)
            cam = GetComponent<Camera>();
        Vector3 pos = cam.WorldToViewportPoint(worldPosition);
        return (checkLeft && pos.x < -margin) ||
            (checkRight && pos.x > 1 + margin) ||
            (checkBottom && pos.y < -margin) ||
            (checkTop && pos.y > 1 + margin) || pos.z < 0;
    }

    private void TriggerDefeat()
    {
        if (defeated) return;
        defeated = true;
        StartCoroutine(DefeatExplode());
    }

    IEnumerator DefeatExplode()
    {
        planeScript.StopPlane(true);
        var sr = planeGO.GetComponent<SpriteRenderer>();
        sr.enabled = false;
        explosion.SetActive(true);
        yield return new WaitForSeconds(.6f);
        StartCoroutine(script3.ShowMenuDefeat());
        script4.SetStars(levelName, 0);
    }

    public void SetCheckSides(bool top, bool bottom, bool left, bool right)
    {
        checkTop = top;
        checkBottom = bottom;
        checkLeft = left;
        checkRight = right;
    }
}
