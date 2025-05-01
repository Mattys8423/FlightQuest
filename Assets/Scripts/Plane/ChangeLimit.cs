using UnityEngine;

public class ChangeLimit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlaneActions>().changeLimit(1.5f);
        }
    }
}
