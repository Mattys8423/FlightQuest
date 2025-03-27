using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private WinCondition script;
    [SerializeField] private GameObject Sparkles;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(gameObject.name + " est entr� en collision avec " + other.gameObject.name);

        // V�rifier si c'est un objet sp�cifique (ex: un joueur)
        if (other.CompareTag("Player"))
        {            
            this.GetComponent<SpriteRenderer>().enabled = false;
            Sparkles.GetComponent<Animator>().SetTrigger("Sparkles");
            script.AddCoin(1);
        }
    }
}
