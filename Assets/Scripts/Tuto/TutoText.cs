using System.Collections;
using TMPro;
using UnityEngine;

public class TutoText : MonoBehaviour
{
    [SerializeField] TMP_Text TexteTuto;
    [SerializeField] GameObject Texte;
    [SerializeField] GameObject fl�che;
    [SerializeField] GameObject fl�che2;

    private bool isHolding = false;
    private float holdTime = 0f;
    private float requiredHoldTime = .8f;
    private float requiredHoldTime2 = 4f;
    private float requiredHoldTime3 = 6f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isHolding = true;
            holdTime = 0f;
        }

        if (isHolding && Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;

            if (holdTime >= requiredHoldTime)
            {
                TexteTuto.SetText("Drag the mouse to see the trajectory of the plane");
            }
            if (holdTime >= requiredHoldTime2)
            {
                TexteTuto.SetText("Your goal is to reach the landing zone");
                Texte.SetActive(true);
                fl�che.SetActive(true);
            }
            if (holdTime >= requiredHoldTime3)
            {
                TexteTuto.SetText("Release the left click to lauch your plane");
                Texte.SetActive(false);
                fl�che.SetActive(false);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isHolding = false;
            Texte.SetActive(false);
            fl�che.SetActive(false);
            StartCoroutine(ShowDJ());
        }
    }

    IEnumerator ShowDJ()
    {
        yield return new WaitForSeconds(1.2f);
        TexteTuto.SetText("press space bar to double jump");
        Time.timeScale = 0f;
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        Time.timeScale = 1f;
        yield return new WaitForSeconds(.3f);
        fl�che2.SetActive(true);
        TexteTuto.SetText("Pay attention to your number of launch it can be useful !");
        yield return new WaitForSeconds(2f);
        fl�che2.SetActive(false);
        TexteTuto.gameObject.SetActive(false);
    }
}
