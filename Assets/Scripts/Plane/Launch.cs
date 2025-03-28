using TMPro;
using UnityEngine;

public class Launch : MonoBehaviour
{
    [SerializeField] TMP_Text TexteNb;
    [SerializeField] PlaneActions script;

    // Update is called once per frame
    void Update()
    {
        TexteNb.text = script.NumberOfLaunch.ToString();
    }
}
