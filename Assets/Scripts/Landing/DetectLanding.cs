using System.Collections;
using UnityEngine;

public class DetectLanding : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(gameObject.name + " est entré en collision avec " + other.gameObject.name);

        // Vérifier si c'est un objet spécifique (ex: un joueur)
        if (other.CompareTag("Player"))
        {
            StartCoroutine(StopVelocity(other));
            Debug.Log("PlayerHasWin");
        }
    }

    IEnumerator StopVelocity(Collider2D other)
    {
        yield return new WaitForSeconds(.1f);
        other.attachedRigidbody.linearVelocity = Vector2.zero;
    }
}
