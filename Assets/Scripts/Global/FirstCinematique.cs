using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstCinematique : MonoBehaviour
{
    [SerializeField] TypewriterEffect1 script;
    [SerializeField] SaveStars script2;
    [SerializeField] GameObject Canvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(script.TypeText());
        StartCoroutine(WaitAndScene());
    }

    // Update is called once per frame
    void Update()
    {
        if (script.isCoroutineRunning == false) { 
            Canvas.SetActive(false);
        }
    }

    IEnumerator WaitAndScene()
    {
        yield return new WaitForSeconds(23.30f);
        if (script2.GetBoolFromCineReviewScene())
        {
            SceneManager.LoadScene("CineReviewScene");
        }
        else
        {
            SceneManager.LoadScene("LevelScene");
        }
    }
}
