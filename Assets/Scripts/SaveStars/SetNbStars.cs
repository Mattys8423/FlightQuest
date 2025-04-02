using UnityEngine;
using TMPro;

public class SetNbStars : MonoBehaviour
{
    [SerializeField] SaveStars script;
    [SerializeField] TMP_Text numStars;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        script.SetTotalStars();
        numStars.SetText(script.GetTotalStars().ToString());
    }
}
