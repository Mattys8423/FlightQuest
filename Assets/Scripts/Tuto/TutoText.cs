using System.Collections;
using TMPro;
using UnityEngine;

public class TutoText : MonoBehaviour
{
    [SerializeField] TMP_Text TexteTuto;
    [SerializeField] GameObject Texte;
    [SerializeField] GameObject flèche;
    [SerializeField] GameObject flèche2;

    private bool isHolding = false;
    private bool hasDJ = false;
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
                flèche.SetActive(true);
            }
            if (holdTime >= requiredHoldTime3)
            {
                TexteTuto.SetText("Release the left click to lauch your plane");
                Texte.SetActive(false);
                flèche.SetActive(false);
            }
        }

        if (Input.GetMouseButtonUp(0) && !hasDJ)
        {
            isHolding = false;
            Texte.SetActive(false);
            flèche.SetActive(false);
            StartCoroutine(ShowDJ());
            hasDJ = true;
        }
    }

    IEnumerator ShowDJ()
    {
        yield return new WaitForSeconds(.8f);
        TexteTuto.SetText("press space bar or click to double jump");
        Time.timeScale = 0f;
        while (!Input.GetMouseButton(0))
        {
            yield return null;
        }
        Time.timeScale = 1f;
        yield return new WaitForSeconds(.3f);
        flèche2.SetActive(true);
        TexteTuto.SetText("Pay attention to your number of launch it can be useful !");
        yield return new WaitForSeconds(2f);
        flèche2.SetActive(false);
        TexteTuto.gameObject.SetActive(false);
    }
}
