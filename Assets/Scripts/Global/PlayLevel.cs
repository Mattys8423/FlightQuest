using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayLevel : MonoBehaviour
{

    public void playlevel()
    {
        StartCoroutine(WaitAndPlayName(gameObject.name));
    }

    IEnumerator WaitAndPlayName(string SceneName)
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneName);
    }
}
