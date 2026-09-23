using System;
using System.Collections;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_actions : MonoBehaviour
{
    public void play(int SceneIndex)
    {
        StartCoroutine(WaitAndPlay(SceneIndex));
    }
    public void PlayFromMenu(string SceneName)
    {
        StartCoroutine(WaitAndPlayName2(SceneName));
    }

    public void playlevel(string SceneName)
    {
        if (ScrollClickGuard.ShouldSuppressClick)
            return;

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

    IEnumerator WaitAndPlayName2(string SceneName)
    {
        yield return new WaitForSeconds(1.20f);
        SceneManager.LoadScene(SceneName);
    }

    IEnumerator WaitAndQuit()
    {
        yield return new WaitForSeconds(.25f);
        Application.Quit();
    }
}
