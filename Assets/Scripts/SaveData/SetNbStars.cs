using UnityEngine;
using TMPro;
using System.Collections;

public class SetNbStars : MonoBehaviour
{
    [SerializeField] SaveStars script;
    [SerializeField] TMP_Text numStars;
    [SerializeField] GameObject GoHangar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (script.GetTotalStars() >= 10 && !script.GetBoolStar())
        {
            GoHangar.SetActive(true);
            script.SetBoolStar();
        }
        script.SetTotalStars();
        numStars.SetText(script.GetTotalStars().ToString());
    }
}
