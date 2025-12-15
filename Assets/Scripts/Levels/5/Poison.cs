using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Poison : MonoBehaviour
{
    [SerializeField] private GameObject Plane;
    [SerializeField] private GameObject Explosion;
    [SerializeField] private PlaneActions script;
    [SerializeField] private EndLevelMenu script3;
    [SerializeField] private SaveStars script4;
    private string LevelName;

    private void Start()
    {
        LevelName = SceneManager.GetActiveScene().name;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(DefeatExplode());
    }

    IEnumerator DefeatExplode()
    {
        script.StopPlane(true);
        Plane.GetComponent<SpriteRenderer>().enabled = false;
        Explosion.SetActive(true);
        yield return new WaitForSeconds(.6f);
        StartCoroutine(script3.ShowMenuDefeat());
        script4.SetStars(LevelName, 0);
        script4.AddDeath();
    }
}
