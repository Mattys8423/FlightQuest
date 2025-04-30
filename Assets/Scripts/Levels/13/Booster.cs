using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] AudioSource Effects;
    [SerializeField] AudioClip Speed;
    [SerializeField] PlaneActions script;
    [SerializeField] float Force = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Effects.PlayOneShot(Speed);
            script.AddForceX(Force);
        }
    }
}
