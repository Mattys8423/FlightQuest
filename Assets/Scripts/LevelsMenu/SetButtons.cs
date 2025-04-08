using UnityEngine;
using UnityEngine.UI;

public class SetButtons : MonoBehaviour
{
    [SerializeField] SaveStars script;
    [SerializeField] private Transform content;
    [SerializeField] private string buttonToKeepName = "LevelTuto";

    private void Start()
    {
        if (script.GetTotalStars() < 1)
        {
            DisableAllButtonsExceptOne();
        }
    }

    void DisableAllButtonsExceptOne()
    {
        Button[] buttons = content.GetComponentsInChildren<Button>();

        foreach (Button btn in buttons)
        {
            if (btn.name != buttonToKeepName)
            {
                GameObject Cadenas = btn.gameObject.transform.Find("Cadenas").gameObject;
                if (Cadenas != null)
                {
                    Cadenas.gameObject.SetActive(true);
                }
                btn.interactable = false;
            }
        }
    }
}
