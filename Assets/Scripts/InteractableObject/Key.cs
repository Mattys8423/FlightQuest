using System.Collections;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private Hangar script;
    [SerializeField] private GameObject Sparkles;
    [SerializeField] AudioSource audioK;
    [SerializeField] AudioClip CoinSuccess;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<PolygonCollider2D>().enabled = false;
            Sparkles.GetComponent<Animator>().SetTrigger("Sparkles");
            audioK.GetComponent<AudioSource>().PlayOneShot(CoinSuccess);
            script.SetBool(true);
        }
    }
}
