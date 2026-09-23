using UnityEngine;
using UnityEngine.UI;

public class EndlessMode : MonoBehaviour
{
    [SerializeField] private SaveStars script;
    [SerializeField] private Button Credits;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (script.GetCine() >= 5)
        {
            Credits.interactable = true;
        }
        else
        {
            Credits.interactable = false;
        }
    }
}
