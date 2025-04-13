using UnityEngine;

public class playSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClipSuccess;
    [SerializeField] private AudioClip _audioClipCant;
    [SerializeField] private ScrollSnapToCenter script;

    public void playsound()
    {
        switch (script.GetLockCurrentImage())
        {
            case true:
                _audioSource.PlayOneShot(_audioClipCant);
                break;
            case false:
                _audioSource.PlayOneShot(_audioClipSuccess);
                break;
        }
    }
}
