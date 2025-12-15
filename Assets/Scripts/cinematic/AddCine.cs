using UnityEngine;

public class AddCine : MonoBehaviour
{
    [SerializeField] SaveStars scripts;

    private void Start()
    {
        scripts.AddCine();
    }
}
