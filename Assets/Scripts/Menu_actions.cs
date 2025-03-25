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

    public void quit()
    {
        StartCoroutine(WaitAndQuit());
    }

    IEnumerator WaitAndPlay(int SceneIndex)
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneIndex);
    }

    IEnumerator WaitAndQuit()
    {
        yield return new WaitForSeconds(.5f);
        Application.Quit();
    }
}
