using UnityEngine;

public class GroundCollide : MonoBehaviour
{
    [SerializeField] private PlaneActions script;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            script.StopPlane();
        }
    }
}
