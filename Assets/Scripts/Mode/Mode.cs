using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mode : MonoBehaviour
{
    public Animator Canvas;
    [SerializeField] private SaveStars script;
    [SerializeField] private Button endlessmode;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Canvas.Play("Out");

        if (script.GetCine() >= 5)
        {
            endlessmode.interactable = true;
        }
        else
        {
            endlessmode.interactable = false;
        }
    }

    public void SetFromModeScene(bool value)
    {
        if (GameInstance.instance == null)
        {
            Debug.LogError("Mode : aucune GameInstance active.", this);
            return;
        }

        GameInstance.instance.FromModeScene = value;
    }
}
