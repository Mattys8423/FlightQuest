using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    [SerializeField] private Button buttonStats;
    [SerializeField] private TMP_Text Deaths;
    [SerializeField] private TMP_Text Time;
    [SerializeField] private SaveStars script;
    private PlayTimeManager script2;
    
    void Start()
    {
        if(script.GetCine() >= 5)
        {
            buttonStats.interactable = true;
        }

        Deaths.text = script.GetDeaths().ToString();
        GameObject timerGO = GameObject.FindWithTag("Timer");

        if (timerGO != null)
        {
            script2 = timerGO.GetComponent<PlayTimeManager>();
            Time.text = script2.GetFormattedTimeHours().ToString() + " H " + script2.GetFormattedTimeHours().ToString() + "M";
        }
    }
}
