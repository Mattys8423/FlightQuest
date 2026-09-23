using UnityEngine;
using UnityEngine.UI;

public class CreditsButton : MonoBehaviour
{
    [SerializeField] private SaveStars script;
    [SerializeField] private GameObject Creditbtn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (script == null)
        {
            Debug.LogError("CreditsButton : renseigne la référence SaveStars dans l'Inspector.", this);
            return;
        }

        if (Creditbtn == null)
        {
            Debug.LogError("CreditsButton : renseigne le bouton des crédits dans l'Inspector.", this);
            return;
        }

        bool storyFinished = script.GetBoolStory();
        Button button = Creditbtn.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("CreditsButton : l'objet renseigné ne contient pas de composant Button.", Creditbtn);
            return;
        }

        button.interactable = storyFinished;
        Creditbtn.SetActive(storyFinished);
    }
}
