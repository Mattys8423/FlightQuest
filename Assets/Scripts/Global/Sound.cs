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
        Main.SetFloat("Master", Mathf.Log10(GameInstance.instance.GetMasterVolume()) * 20);
        Main.SetFloat("Music", Mathf.Log10(GameInstance.instance.GetMusicVolume()) * 20);
        Main.SetFloat("Effects", Mathf.Log10(GameInstance.instance.GetEffectsVolume()) * 20);
    }
}
