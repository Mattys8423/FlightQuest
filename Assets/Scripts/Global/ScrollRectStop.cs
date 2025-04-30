using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectStop : MonoBehaviour
{
    [SerializeField] ScrollSnapToCenter script;
    [SerializeField] InformationsDisplay script2;
    [SerializeField] SaveStars script3;
    [SerializeField] GameObject[] SelectedLogo;
    public ScrollRect scrollRect;
    public int PlaneNumber;

    private float timePassed = 0f;

    // Update is called once per frame
    void Update()
    {
        if (scrollRect.velocity.magnitude > 100f)
        {
            for (int i = 0; i < SelectedLogo.Length; i++)
            {
                SelectedLogo[i].SetActive(false);
            }
        }
        if (scrollRect.velocity.magnitude < 1)
        {
            PlaneNumber = script.ItemNumber;
            SelectedLogo[script3.GetPlane()].SetActive(true);
        }
        if (scrollRect.velocity.magnitude == 0f)
        {
            timePassed += Time.deltaTime;

            if (timePassed == 1f)
            {
                script2.SetBool(true);
            }
        }
        else
        {
            timePassed = 0f;
        }
    }
}
