using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cinematic : MonoBehaviour
{
    [SerializeField] private SaveStars script;
    private string LevelName;

    private void Start()
    {
        if (script.GetBoolFC() == false)
        {
            script.SetBoolFC();
            SceneManager.LoadScene("FirstCinematique");
        }
    }
}
