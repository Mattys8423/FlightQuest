using UnityEngine;

public class AddCine : MonoBehaviour
{
    [SerializeField] SaveStars scripts;
    [SerializeField] private int NumberOfCinematicWatch;

    private void Start()
    {
        if (scripts.GetCine() == NumberOfCinematicWatch) scripts.AddCine();
    }
}
