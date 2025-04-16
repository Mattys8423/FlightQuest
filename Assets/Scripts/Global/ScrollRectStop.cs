using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectStop : MonoBehaviour
{
    [SerializeField] ScrollSnapToCenter script;
    [SerializeField] InformationsDisplay script2;
    public ScrollRect scrollRect;
    public int PlaneNumber;

    private float timePassed = 0f;

    // Update is called once per frame
    void Update()
    {
        if (scrollRect.velocity.magnitude < 1)
        {
            PlaneNumber = script.ItemNumber;
        }
        if (scrollRect.velocity.magnitude == 0f)
        {
            timePassed += Time.deltaTime;

            if (timePassed == 2f)
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
