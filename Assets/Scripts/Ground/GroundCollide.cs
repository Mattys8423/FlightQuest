using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundCollide : MonoBehaviour
{
    [SerializeField] private GameObject Plane;
    [SerializeField] private GameObject Explosion;
    [SerializeField] private PlaneActions script;
    [SerializeField] private WinCondition script2;
    [SerializeField] private EndLevelMenu script3;
    [SerializeField] private SaveStars script4;
    private string LevelName;

    private void Start()
    {
        LevelName = SceneManager.GetActiveScene().name;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        this.gameObject.GetComponent<PolygonCollider2D>().isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            script.SetGrounded(true);
            script.StopPlane(false);
            if (Plane.transform.rotation.eulerAngles.z > 40f && Plane.transform.rotation.eulerAngles.z < 320f)
            {
                StartCoroutine(DefeatExplode());
            }
            else
            {
                switch (script2.GetCondition())
                {
                    case true:
                        StartCoroutine(script3.ShowMenuVictory());
                        script4.SetStars(LevelName, script2.GetCoin());
                        break;
                    case false:
                        if (script.NumberOfLaunch > 0) 
                        {
                            break;
                        }
                        else
                        {
                            StartCoroutine(script3.ShowMenuDefeat());
                            script4.SetStars(LevelName, 0);
                            break;
                        }
                }
            }
            this.gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
        }
    }

    IEnumerator DefeatExplode()
    {
        Plane.GetComponent<SpriteRenderer>().enabled = false;
        Explosion.SetActive(true);
        yield return new WaitForSeconds(.6f);
        StartCoroutine(script3.ShowMenuDefeat());
        script4.SetStars(LevelName, 0);
    }
}

