using System.Collections;
using System.Linq;
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
        StartCoroutine(DisableButtonLevel());
    }

    void DisableAllButtonsExceptOne()
    {
        Button[] buttons = content.GetComponentsInChildren<Button>().Where(b => b.gameObject.name != "GalaxyEasterEgg").ToArray();

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

    IEnumerator DisableButtonLevel()
    {
        yield return new WaitForSeconds(.2f);
        Button[] buttons = content.GetComponentsInChildren<Button>()
        .Where(b => b.gameObject.name != "GalaxyEasterEgg")
        .ToArray();

        // On filtre et trie les boutons selon leur numéro de niveau
        var sortedButtons = buttons
            .OrderBy(b => b.GetComponent<PlayLevel>().GetLevelCount())
            .ToList();

        for (int i = 0; i < sortedButtons.Count; i++)
        {
            if (i == 0) continue;

            bool previousPassed = sortedButtons[i - 1].GetComponent<PlayLevel>().GetLevelPass();
            if (!previousPassed)
            {
                sortedButtons[i].interactable = false;
                GameObject Cadenas = sortedButtons[i].gameObject.transform.Find("Cadenas").gameObject;
                if (Cadenas != null)
                {
                    Cadenas.gameObject.SetActive(true);
                }
            }
        }
    }
}
