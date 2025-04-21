using UnityEngine;
using System.Collections;

public class StopZone : MonoBehaviour
{
    [SerializeField] GameObject Canvas;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(StopVelocity(collision));
            Canvas.SetActive(false);
        }
    }

    IEnumerator StopVelocity(Collider2D other)
    {
        yield return new WaitForSeconds(.2f);
        other.attachedRigidbody.linearVelocity = Vector2.zero;
    }
}
