using UnityEngine;
using UnityEngine.UI;

public class CineReviewManager : MonoBehaviour
{
    [SerializeField] private SaveStars script;
    private int CinematicCount = 0;

    private void Start()
    {
        script.SetBoolFromCineReviewScene(false);
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject ActualChild = transform.GetChild(i).gameObject;
            int childCountOfChild = ActualChild.transform.childCount;

            for ( int j = 0; j < childCountOfChild; j++)
            {
                if (CinematicCount < script.GetCine())
                {
                    ActualChild.transform.GetChild(j).gameObject.GetComponent<Button>().interactable = true;
                    ActualChild.transform.GetChild(j).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    CinematicCount++;
                }
                else
                {
                    ActualChild.transform.GetChild(j).gameObject.GetComponent<Button>().interactable = false;
                    ActualChild.transform.GetChild(j).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    CinematicCount++;
                }
            }
        }
    }
}
