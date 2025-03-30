using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SliderSound : MonoBehaviour
{
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider EffectsSlider;
    [SerializeField] private AudioMixer Main;

    void Update()
    {
        MusicSlider.value = GameInstance.instance.GetMusicVolume();
        MasterSlider.value = GameInstance.instance.GetMasterVolume();
        EffectsSlider.value = GameInstance.instance.GetEffectsVolume();
    }

    public void ChangeVolumeMaster()
    {
        Main.SetFloat("Master", Mathf.Log10(MasterSlider.value) * 20);
        GameInstance.instance.SetEffectsVolume(MasterSlider.value);
    }

    public void ChangeVolumeMusic()
    {
        Main.SetFloat("Music", Mathf.Log10(MusicSlider.value)*20);
        GameInstance.instance.SetEffectsVolume(MusicSlider.value);
    }

    public void ChangeVolumeEffects()
    {
        Main.SetFloat("Effects", Mathf.Log10(EffectsSlider.value) * 20);
        GameInstance.instance.SetEffectsVolume(EffectsSlider.value);
    }
}
