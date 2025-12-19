using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundSetOutSpace : MonoBehaviour
{
    [SerializeField] private PlaneActions script;
    [SerializeField] private SaveStars script2;
    [SerializeField] private Animator Transition;
    [SerializeField] private GameObject Change;

    private bool CanSeeCine;
    private string LevelName;

    private void Start()
    {
        LevelName = SceneManager.GetActiveScene().name;
        switch (script2.GetBoolCineFalseEnd())
        {
            case true:
                if (script2.GetCine() >= 6)
                {
                    Change.SetActive(true);
                    gameObject.SetActive(false);
                    CanSeeCine = false;
                }
                else
                {
                    Change.SetActive(false);
                    gameObject.SetActive(true);
                    CanSeeCine = true; 
                    break;
                }
                    break;
            case false:
                if (script2.GetCine() >= 6)
                {
                    Change.SetActive(true);
                    gameObject.SetActive(false);
                    CanSeeCine = false;
                }
                else
                {
                    Change.SetActive(false);
                    gameObject.SetActive(true);
                    CanSeeCine = true;
                    break;
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && CanSeeCine)
        {
            StartCoroutine(WaitAndPlayCine());
        }
    }

    IEnumerator WaitAndPlayCine()
    {
        Transition.Play("helice");
        yield return new WaitForSeconds(1.30f);
        script.SetGrounded(true);
        script.StopPlane(false);
        script2.SetBoolUnlocked();
        SceneManager.LoadScene("Cinematique");
    }
}

