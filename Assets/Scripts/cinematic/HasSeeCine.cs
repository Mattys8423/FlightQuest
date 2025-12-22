using UnityEngine;

public class HasSeenCine : MonoBehaviour
{
    [SerializeField] private SaveStars script;
    [SerializeField] private int NumberCine;

    void Awake()
    {
     if (script.GetCine() >= NumberCine)
        {
            gameObject.GetComponent<CinematicManager>().enabled = false;
        }   
    }
}
