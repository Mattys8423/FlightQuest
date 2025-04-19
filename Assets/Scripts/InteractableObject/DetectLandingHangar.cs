using System.Collections;
using UnityEngine;

public class DetectLandingHangar : MonoBehaviour
{
    [SerializeField] private WinCondition script;
    [SerializeField] private Hangar script2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && script2.GetBool() == true)
        {
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
