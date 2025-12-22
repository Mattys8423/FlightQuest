using UnityEngine;
using UnityEngine.UI;

public class CineReviewManager : MonoBehaviour
{
    [SerializeField] private SaveStars script;
    [SerializeField] private int NumberCinematic;

    private void Start()
    {
        switch (NumberCinematic) 
        { 
            case(3):
                if (script.GetCine() >= NumberCinematic && script.GetBoolCineFalseEnd())
                {
                    gameObject.GetComponent<Button>().interactable = true;
                    gameObject.GetComponent<Transform>().GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    gameObject.GetComponent<Button>().interactable = false;
                    gameObject.GetComponent<Transform>().GetChild(0).gameObject.SetActive(false);
                }
                break;
            case (4):
                if (script.GetCine() >= NumberCinematic && script.GetBoolCineSpace())
                {
                    gameObject.GetComponent<Button>().interactable = true;
                    gameObject.GetComponent<Transform>().GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    gameObject.GetComponent<Button>().interactable = false;
                    gameObject.GetComponent<Transform>().GetChild(0).gameObject.SetActive(false);
                }
                break;
            default:
                if (script.GetCine() >= NumberCinematic)
                {
                    gameObject.GetComponent<Button>().interactable = true;
                    gameObject.GetComponent<Transform>().GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    gameObject.GetComponent<Button>().interactable = false;
                    gameObject.GetComponent<Transform>().GetChild(0).gameObject.SetActive(false);
                }
                break;
        }
    }
}
