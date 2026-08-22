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
        // Recalcule d'abord le total pour corriger les anciennes sauvegardes qui
        // ont pu compter plusieurs fois les étoiles d'un même niveau.
        script.SetTotalStars();

        if (script.GetTotalStars() >= 10 && !script.GetBoolTeStar())
        {
            script.SetBoolFromCinematic(true);
            GoHangar.SetActive(true);
            script.SetBoolTeStar();
        }
        if (script.GetTotalStars() >= 20 && !script.GetBoolTwStar())
        {
            script.SetBoolFromCinematic(true);
            GoHangar.SetActive(true);
            script.SetBoolTwStar();
        }
        if (script.GetTotalStars() >= 30 && !script.GetBoolThStar())
        {
            script.SetBoolFromCinematic(true);
            GoHangar.SetActive(true);
            script.SetBoolThStar();
        }
        numStars.SetText(script.GetTotalStars().ToString());
    }
}
