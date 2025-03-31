using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    [SerializeField] private AudioMixer Main;

    void Start()
    {
        Debug.Log(GameInstance.instance.GetMusicVolume());
        Debug.Log(GameInstance.instance.GetMasterVolume());
        Debug.Log(GameInstance.instance.GetEffectsVolume());
        Main.SetFloat("Master", GameInstance.instance.GetMasterVolume());
        Main.SetFloat("Music", GameInstance.instance.GetMusicVolume());
        Main.SetFloat("Effects", GameInstance.instance.GetEffectsVolume());
    }
}
