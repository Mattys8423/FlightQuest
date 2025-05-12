using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetSettingsLayout : MonoBehaviour
{
    [SerializeField] GameObject Settings;
    [SerializeField] GameObject VolumeSettings;
    [SerializeField] GameObject Sound;
    [SerializeField] GameObject Parametres;
    [SerializeField] GameObject Controls;
    private bool IsActivate = false;

    // Update is called once per frame
    void Update()
    {
        if(Settings.activeSelf && !IsActivate)
        {
            EventSystem.current.SetSelectedGameObject(VolumeSettings);
            Sound.SetActive(true);
            Parametres.SetActive(false);
            Controls.SetActive(false);
            IsActivate = true;
        }
        if (!Settings.activeSelf && IsActivate)
        {
            IsActivate = false;
        }
    }
}
