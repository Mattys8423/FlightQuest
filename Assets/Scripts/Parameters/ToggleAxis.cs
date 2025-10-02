using UnityEngine;
using UnityEngine.UI;

public class ToggleAxis : MonoBehaviour
{
    [SerializeField] private GameObject Controls;
    [SerializeField] private Toggle toggleAxis;
    [SerializeField] private SaveStars Script;
    private bool ControlHide;
    private bool previousState;

    void Start()
    {
        previousState = Controls.activeSelf;
        ControlHide = !Controls.activeSelf;
    }

    void Update()
    {
        if (Controls.activeSelf != previousState)
        {
            ControlHide = !ControlHide;
            previousState = Controls.activeSelf;
            toggleAxis.isOn = Script.GetBoolInversed();
        }

        if (!ControlHide)
        {
             Script.SetBoolInversed(toggleAxis.isOn);
        }
    }
}
