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
        Vector3 pos = cam.WorldToViewportPoint(planeGO.transform.position);

        //Check with margin if the plane is in the camera bounds
        if (pos.x < 0 - margin || pos.x > 1 + margin || pos.y < 0 - margin || pos.y > 1 + margin)
        {
            TriggerDefeat();
        }

        //or behind
        if (pos.z < 0)
        {
            TriggerDefeat();
            return;
        }
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
}
