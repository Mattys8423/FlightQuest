using UnityEngine;

public class HasSeenCine : MonoBehaviour
{
    [SerializeField] private SaveStars script;
    [SerializeField] private int NumberCine;

    void Start()
    {
     if (script.GetCine() >= NumberCine)
        {
            gameObject.GetComponent<CinematicManager>().enabled = false;
        }   
    }
}
