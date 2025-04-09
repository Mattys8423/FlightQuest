using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_actions : MonoBehaviour
{
    public void play(int SceneIndex)
    {
        StartCoroutine(WaitAndPlay(SceneIndex));
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
