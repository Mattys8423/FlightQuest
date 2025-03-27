using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private WinCondition script;
    [SerializeField] private GameObject Sparkles;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(gameObject.name + " est entré en collision avec " + other.gameObject.name);

        // Vérifier si c'est un objet spécifique (ex: un joueur)
        if (other.CompareTag("Player"))
        {            
            this.GetComponent<SpriteRenderer>().enabled = false;
            Sparkles.GetComponent<Animator>().SetTrigger("Sparkles");
            script.AddCoin(1);
        }
    }
}
