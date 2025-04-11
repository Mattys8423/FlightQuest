using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectStop : MonoBehaviour
{
    [SerializeField] ScrollSnapToCenter script;
    public ScrollRect scrollRect;
    public int PlaneNumber;

    // Update is called once per frame
    void Update()
    {
        if (scrollRect.velocity.magnitude < 1)
        {
            PlaneNumber = script.ItemNumber;
        }
    }
}
