using System.Collections;
using UnityEngine;

public class DetectLanding : MonoBehaviour
{
    [SerializeField] private WinCondition script;
    [SerializeField] AudioClip LandingClip;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(LandingClip);
            StartCoroutine(StopVelocity(other));
            script.SetCondition(true);
        }
    }

    IEnumerator StopVelocity(Collider2D other)
    {
        yield return new WaitForSeconds(.2f);
        other.attachedRigidbody.linearVelocity = Vector2.zero;
    }
}
