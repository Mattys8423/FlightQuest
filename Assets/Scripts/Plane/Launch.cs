using TMPro;
using UnityEngine;

public class Launch : MonoBehaviour
{
    [SerializeField] TMP_Text TexteNb;
    [SerializeField] TMP_Text TexteDJ;
    [SerializeField] PlaneActions script;
    private Vector3 fixedPosition;

    void Start()
    {
        fixedPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        TexteNb.text = script.NumberOfLaunch.ToString();
        if (script.GetDJ())
        {
            TexteDJ.color = Color.red;
        }
    }

    void LateUpdate()
    {
        transform.position = fixedPosition;
    }
}
