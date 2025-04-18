using System.Collections;
using UnityEngine;

public class playSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClipSuccess;
    [SerializeField] private AudioClip _audioClipCant;
    [SerializeField] private ScrollSnapToCenter script;
    [SerializeField] private GameObject _gameObjectTick;
    [SerializeField] private GameObject _gameObjectCross;

    public void apply()
    {
        switch (script.GetLockCurrentImage())
        {
            case true:
                _audioSource.PlayOneShot(_audioClipCant);
                StartCoroutine(Feedback(_gameObjectCross));
                break;
            case false:
                _audioSource.PlayOneShot(_audioClipSuccess);
                StartCoroutine(Feedback(_gameObjectTick));
                break;
        }
    }

    IEnumerator Feedback(GameObject Feedback)
    {
        Feedback.SetActive(true);
        yield return new WaitForSeconds(.5f);
        Feedback.SetActive(false);
    }
}
