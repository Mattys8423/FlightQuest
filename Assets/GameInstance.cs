using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class GameInstance : MonoBehaviour
{
    public static GameInstance instance;
    [SerializeField] private AudioMixer Main;
    private float VolumeMusic = 0.5f;
    private float VolumeEffects = 0.5f;
    private float VolumeMaster = 0.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float GetMusicVolume() { return VolumeMusic; }
    public float GetEffectsVolume() {  return VolumeEffects; }
    public float GetMasterVolume() {  return VolumeMaster; }

    public void SetMusicVolume(float volume) { VolumeMusic = volume;}
    public void SetEffectsVolume(float volume) { VolumeEffects = volume;}
    public void SetMasterVolume(float volume) { VolumeMaster = volume;}
}
