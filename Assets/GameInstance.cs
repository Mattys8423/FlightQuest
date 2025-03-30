using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class GameInstance : MonoBehaviour
{
    public static GameInstance instance;
    private float VolumeMusic;
    private float VolumeEffects;
    private float VolumeMaster;

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
