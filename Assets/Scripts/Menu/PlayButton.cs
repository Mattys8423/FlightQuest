using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private SaveStars script;

    public void ButtonPlay()
    {
        StartCoroutine(WaitAndScene());
    }

    IEnumerator WaitAndScene()
    {
        yield return new WaitForSeconds(1.20f);
        if (script.GetCine() == 0)
        {
            SceneManager.LoadScene("FirstCinematique");
        }
        else
        {
            SceneManager.LoadScene("LevelScene");
        }
    }
}
