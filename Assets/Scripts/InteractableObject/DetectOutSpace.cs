using System.Collections;
using UnityEngine;

public class DetectOutSpace : MonoBehaviour
{
    [SerializeField] private WinCondition script;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(StopVelocity(other));
            script.SetCondition(true);
        }
    }

    IEnumerator StopVelocity(Collider2D other)
    {
        yield return new WaitForSeconds(.2f);
        other.attachedRigidbody.linearVelocity = Vector2.zero;
        other.attachedRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
