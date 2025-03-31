using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private WinCondition script;
    [SerializeField] private GameObject Sparkles;
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip CoinSuccess;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {            
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<CircleCollider2D>().enabled = false;
            Sparkles.GetComponent<Animator>().SetTrigger("Sparkles");
            audio.PlayOneShot(CoinSuccess);
            script.AddCoin(1);
            Debug.Log(script.GetCoin());
        }
    }
}
