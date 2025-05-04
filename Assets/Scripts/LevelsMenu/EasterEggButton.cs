using UnityEngine;
using UnityEngine.UI;

public class EasterEggButton : MonoBehaviour
{
    [SerializeField] private SaveStars script;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (script.GetBoolUnlocked())
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
        else { gameObject.GetComponent<Button>().interactable = false; }
    }
}
