using UnityEngine;

public class AddCineFalseEnd : MonoBehaviour
{
    [SerializeField] SaveStars scripts;

    private void Start()
    {
        switch (scripts.GetBoolCineSpace())
        {
            case true:
                if (scripts.GetCine() == 3) scripts.AddCine(); scripts.SetBoolCineFalseEnd();
                break;
            case false:
                if (scripts.GetCine() == 2) scripts.AddCine(); scripts.SetBoolCineFalseEnd();
                break;
        }
    }
}
