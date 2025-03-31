using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SliderSound : MonoBehaviour
{
    [SerializeField] GameObject MenuSettings;
    private bool hasbeeninitialized = false;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider EffectsSlider;
    [SerializeField] private AudioMixer Main;

    void Update()
    {
        if (MenuSettings.activeSelf && !hasbeeninitialized)
        {
            MusicSlider.value = GameInstance.instance.GetMusicVolume();
            MasterSlider.value = GameInstance.instance.GetMasterVolume();
            EffectsSlider.value = GameInstance.instance.GetEffectsVolume();
            hasbeeninitialized = true;
        }
    }

    public void ChangeVolumeApply()
    {
        Main.SetFloat("Master", Mathf.Log10(MasterSlider.value) * 20);
        GameInstance.instance.SetMasterVolume(MasterSlider.value);

        Main.SetFloat("Music", Mathf.Log10(MusicSlider.value)*20);
        GameInstance.instance.SetMusicVolume(MusicSlider.value);

        Main.SetFloat("Effects", Mathf.Log10(EffectsSlider.value) * 20);
        GameInstance.instance.SetEffectsVolume(EffectsSlider.value);
    }

    public void SetHasBeenInitialized(bool choice)
    {
        hasbeeninitialized = choice;
    }
}
