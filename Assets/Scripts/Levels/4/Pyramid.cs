using UnityEngine;

public class Pyramid : MonoBehaviour
{
    [SerializeField] GameObject PyramidHole;
    [SerializeField] GameObject PyramidBase;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PyramidBase.SetActive(true);
            PyramidHole.SetActive(false);
        }
    }
}
