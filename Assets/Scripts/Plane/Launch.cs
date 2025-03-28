using TMPro;
using UnityEngine;

public class Launch : MonoBehaviour
{
    [SerializeField] TMP_Text TexteNb;
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
    }

    void LateUpdate()
    {
        transform.position = fixedPosition;
    }
}
