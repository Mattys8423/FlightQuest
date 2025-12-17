using UnityEngine;

public class AddCineSpace : MonoBehaviour
{
    [SerializeField] SaveStars scripts;

    private void Start()
    {
        switch (scripts.GetBoolCineFalseEnd())
        {
            case true:
                if (scripts.GetCine() == 3) scripts.AddCine(); scripts.SetBoolCineSpace();
                break;
            case false:
                if (scripts.GetCine() == 2) scripts.AddCine(); scripts.SetBoolCineSpace();
                break;
        }
    }
}
