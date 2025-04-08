using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] AudioSource Effects;
    [SerializeField] AudioClip Boing;
    [SerializeField] PlaneActions script;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<Animator>().SetTrigger("Boing");
            Effects.PlayOneShot(Boing);
            script.AddForceY(5);
        }
    }
}
