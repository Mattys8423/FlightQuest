using UnityEngine;

public class SetTouchCircle : MonoBehaviour
{
    [SerializeField] TutoText text;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            text.SetCircle(true);
        }
    }
}
