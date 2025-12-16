using System.Collections;
using TMPro;
using UnityEngine;

public class TutoText : MonoBehaviour 
{ 
    [SerializeField] TMP_Text TexteTuto; 
    [SerializeField] GameObject Texte; 
    [SerializeField] GameObject flèche; 
    [SerializeField] GameObject Tap; 
    [SerializeField] GameObject Click; 
    [SerializeField] WinCondition script; 
    
    private bool isHolding = false; 
    private bool hasDJ = false; 
    private bool hasTouchedCircle = false; 
    private float holdTime = 0f; 
    
    [SerializeField] private float requiredHoldTime = .8f; 
    [SerializeField] private float requiredHoldTime2 = 4f; 
    [SerializeField] private float requiredHoldTime3 = 6f; 


    void Update() 
    { 
        if (Input.GetMouseButtonDown(0)) 
        { 
            isHolding = true; holdTime = 0f; 
        } 
        if (isHolding && Input.GetMouseButton(0)) 
        { holdTime += Time.deltaTime; 
            if (holdTime >= requiredHoldTime) 
            { 
                TexteTuto.SetText("Drag to see the trajectory of the plane"); 
                Tap.GetComponent<Animator>().SetTrigger("Dragg"); 
            } 
            if (holdTime >= requiredHoldTime2) 
            { 
                TexteTuto.SetText("Your goal is to reach the landing zone"); 
                Texte.SetActive(true); flèche.SetActive(true);
            }
            if (holdTime >= requiredHoldTime3) { TexteTuto.SetText("Release the click to launch your plane");
                Texte.SetActive(false);
                flèche.SetActive(false);
                Tap.GetComponent<Animator>().SetTrigger("Release");
            }
        }
        if (Input.GetMouseButtonUp(0) && !hasDJ) 
        { 
            isHolding = false;
            Texte.SetActive(false);
            flèche.SetActive(false);
            Tap.SetActive(false);
            StartCoroutine(ShowDJ());
            hasDJ = true;
        }
    }
    IEnumerator ShowDJ() 
    { 
        while (script.GetCoin() < 2 && !hasTouchedCircle) 
        { 
            yield return null;
        }
        Click.SetActive(true);
        TexteTuto.SetText("click to use your skill");
        Time.timeScale = 0f;
        while (!Input.GetMouseButton(0)) 
        { 
            yield return null;
        }
        Time.timeScale = 1f;
        yield return new WaitForSeconds(.3f);
        TexteTuto.SetText("Pay attention to your number of launch and your skill active it can be useful !");
        yield return new WaitForSeconds(3f);
        Click.SetActive(false);
        Tap.SetActive(false);
        TexteTuto.gameObject.SetActive(false);
    }

    public void SetHasDj() 
    { 
        if (!hasDJ) 
        { 
            hasDJ = false;
        }
    }
    
    public void SetCircle(bool answer) 
    { 
        hasTouchedCircle = answer;
    }
}
