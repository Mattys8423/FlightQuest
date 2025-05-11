using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class ScrollSnapToCenter : MonoBehaviour
{
    [SerializeField] SaveStars script;

    public ScrollRect scrollRect;
    public RectTransform content;
    public RectTransform SampleListItem;
    public HorizontalLayoutGroup horizontalLayoutGroup;
    public TMP_Text Name;

    public string[] ItemNames;

    private bool IsSnapped;

    public float snapForce;
    public int ItemNumber;
    float snapSpeed;

    private void Start()
    {
        IsSnapped = false;
        StartCoroutine(ScrollStart());
    }

    void Update()
    {
        int currentItem = Mathf.RoundToInt((0 - content.localPosition.x / (SampleListItem.rect.width + horizontalLayoutGroup.spacing)));
        ItemNumber = currentItem;

        if (scrollRect.velocity.magnitude < 200)
        {
            scrollRect.velocity = Vector2.zero;
            snapSpeed += snapForce * Time.deltaTime;
            content.localPosition = new Vector3(
                Mathf.MoveTowards(content.localPosition.x, 0 - (currentItem * (SampleListItem.rect.width + horizontalLayoutGroup.spacing)), snapSpeed),
                content.localPosition.y,
                content.localPosition.z);
            Name.text = ItemNames[currentItem];
            if (content.localPosition.x == 0 - (currentItem) * (SampleListItem.rect.width + horizontalLayoutGroup.spacing))
            {
                IsSnapped = true;
            }
        }
        if (scrollRect.velocity.magnitude > 200)
        {
            Name.text = "  ";
            IsSnapped = false;
            snapSpeed = 0;
        }
    }

    public void Apply()
    {
        Image currentImage = content.GetChild(ItemNumber).GetComponent<Image>();

        if (currentImage != null)
        {
            if (!currentImage.GetComponent<Lock>().GetIsLocked())
            {
                script.SetPlane(ItemNumber);
            }
        }
    }

    public bool GetLockCurrentImage()
    {
        Image currentImage = content.GetChild(ItemNumber).GetComponent<Image>();
        return currentImage.GetComponent<Lock>().GetIsLocked();
    }

    private IEnumerator ScrollStart()
    {
        yield return new WaitForSeconds(0.5f);
        scrollRect.velocity = new Vector2(-(3700 * script.GetPlane()), 0);
    }
}
