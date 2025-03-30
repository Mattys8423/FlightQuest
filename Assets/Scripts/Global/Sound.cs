using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    [SerializeField] private AudioMixer Main;

    void Start()
    {
        Main.SetFloat("Master", GameInstance.instance.GetMasterVolume());
        Main.SetFloat("Music", GameInstance.instance.GetMusicVolume());
        Main.SetFloat("Effects", GameInstance.instance.GetEffectsVolume());
    }
}
