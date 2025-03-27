using System.Collections;
using UnityEngine;

public class DetectLanding : MonoBehaviour
{
    [SerializeField] private WinCondition script;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(gameObject.name + " est entré en collision avec " + other.gameObject.name);

        // Vérifier si c'est un objet spécifique (ex: un joueur)
        if (other.CompareTag("Player"))
        {
            StartCoroutine(StopVelocity(other));
            script.SetCondition(true);
        }
    }

    IEnumerator StopVelocity(Collider2D other)
    {
        yield return new WaitForSeconds(.1f);
        other.attachedRigidbody.linearVelocity = Vector2.zero;
    }
}
