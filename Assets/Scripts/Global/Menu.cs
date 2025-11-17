using UnityEngine;

public class Menu : MonoBehaviour
{
    public Animator Landscape;
    public Animator Canvas;
    public Animator Titre;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Landscape.Play("bgStart");
        Canvas.Play("ButtonsStart");
        Titre.Play("flightQuestStart");
    }
}
