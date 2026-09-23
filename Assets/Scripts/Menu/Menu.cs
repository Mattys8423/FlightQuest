using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private SaveStars script;
    public Animator Landscape;
    public Animator Canvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        script.SetBoolFromCinematic(false);
        script.SetBoolFromCineReviewScene(false);
        if (!GameInstance.instance.FromModeScene)
        {
            Landscape.Play("bgStart");
        }
        else
        {
            GameInstance.instance.FromModeScene = false;
        }
        Canvas.Play("ButtonsStart");
    }
}
