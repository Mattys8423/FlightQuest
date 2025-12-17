using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundSetOutSpace : MonoBehaviour
{
    [SerializeField] private PlaneActions script;
    [SerializeField] private SaveStars script2;
    [SerializeField] private Animator Transition;
    private string LevelName;

    private void Start()
    {
        LevelName = SceneManager.GetActiveScene().name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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

