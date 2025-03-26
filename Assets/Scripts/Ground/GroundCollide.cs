using UnityEngine;

public class GroundCollide : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.rigidbody.isKinematic = true;
            collision.rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
            collision.rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
