using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cinematic : MonoBehaviour
{
    [SerializeField] private SaveStars script;
    private string LevelName;

    private void Start()
    {
        if (script.GetCine() > 1)
        {
            script.AddCine();
            SceneManager.LoadScene("FirstCinematique");
        }
    }
}
