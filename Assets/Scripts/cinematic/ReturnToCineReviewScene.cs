using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToCineReviewScene : MonoBehaviour
{
    [SerializeField] private float SecondsBeforeReturn;

    private void Start()
    {
        StartCoroutine(ReturnToScene());
    }

    IEnumerator ReturnToScene()
    {
        yield return new WaitForSeconds(SecondsBeforeReturn);
        SceneManager.LoadScene("CineReviewScene");
    }
}
