using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] AudioSource Effects;
    [SerializeField] AudioClip Speed;
    [SerializeField] PlaneActions script;
    [SerializeField] float Force = 10;

    private bool HasBoost = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !HasBoost)
        {
            Effects.PlayOneShot(Speed);
            script.AddForceX(Force);
            HasBoost = true;
        }
    }
}
