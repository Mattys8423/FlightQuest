using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_actions : MonoBehaviour
{
    [SerializeField] SaveStars SaveStars;

    public void play(int SceneIndex)
    {
        StartCoroutine(WaitAndPlay(SceneIndex));
    }
    public void ChooseScenePlay()
    {
        StartCoroutine(WaitAndScene());
    }

    public void playlevel(string SceneName)
    {
        StartCoroutine(WaitAndPlayName(SceneName));
    }

    public void retry()
    {
        StartCoroutine(WaitAndPlay(SceneManager.GetActiveScene().buildIndex));
    }

    public void nextLevel()
    {
        StartCoroutine(WaitAndPlay(SceneManager.GetActiveScene().buildIndex+1));
    }

    public void quit()
    {
        StartCoroutine(WaitAndQuit());
    }

    IEnumerator WaitAndPlay(int SceneIndex)
    {
        yield return new WaitForSeconds(.25f);
        SceneManager.LoadScene(SceneIndex);
    }

    IEnumerator WaitAndScene()
    {
        yield return new WaitForSeconds(1.20f);
        if (SaveStars.GetCine() < 1)
        {
            SceneManager.LoadScene("FirstCinematique");
        }
        else
        {
            SceneManager.LoadScene("LevelScene");
        }
    }


    IEnumerator WaitAndPlayName(string SceneName)
    {
        yield return new WaitForSeconds(.25f);
        SceneManager.LoadScene(SceneName);
    }

    IEnumerator WaitAndQuit()
    {
        yield return new WaitForSeconds(.25f);
        Application.Quit();
    }
}
