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

    [SerializeField, Min(0f)] private float initialScrollDuration = 0.6f;

    public float snapForce;
    public int ItemNumber;
    float snapSpeed;
    private bool isAutoScrolling;

    private void Start()
    {
        StartCoroutine(InitializeScrollPosition());
    }

    void Update()
    {
        int itemCount = GetItemCount();
        if (itemCount == 0)
        {
            return;
        }

        int currentItem = GetClosestItemIndex(itemCount);
        ItemNumber = currentItem;

        if (isAutoScrolling)
        {
            SetDisplayedName(currentItem);
            return;
        }

        if (scrollRect.velocity.magnitude < 200)
        {
            scrollRect.velocity = Vector2.zero;
            snapSpeed += snapForce * Time.deltaTime;
            float targetX = GetItemPositionX(currentItem);
            content.localPosition = new Vector3(
                Mathf.MoveTowards(content.localPosition.x, targetX, snapSpeed),
                content.localPosition.y,
                content.localPosition.z);
            SetDisplayedName(currentItem);
        }
        else
        {
            Name.text = "  ";
            snapSpeed = 0;
        }
    }

    public void Apply()
    {
        if (ItemNumber < 0 || ItemNumber >= content.childCount)
        {
            return;
        }

        Image currentImage = content.GetChild(ItemNumber).GetComponent<Image>();

        if (currentImage != null)
        {
            Lock planeLock = currentImage.GetComponent<Lock>();
            if (planeLock != null && !planeLock.GetIsLocked())
            {
                script.SetPlane(ItemNumber);
            }
        }
    }

    public bool GetLockCurrentImage()
    {
        if (ItemNumber < 0 || ItemNumber >= content.childCount)
        {
            return true;
        }

        Image currentImage = content.GetChild(ItemNumber).GetComponent<Image>();
        Lock planeLock = currentImage != null ? currentImage.GetComponent<Lock>() : null;
        return planeLock == null || planeLock.GetIsLocked();
    }

    private IEnumerator InitializeScrollPosition()
    {
        bool comesFromCinematic = script.GetBoolFromCinematic();
        if (comesFromCinematic)
        {
            script.SetBoolFromCinematic(false);
        }

        // Attend la construction du layout afin que les tailles utilisées pour
        // centrer les avions soient définitives, quelle que soit la résolution.
        yield return null;
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);

        int itemCount = GetItemCount();
        if (itemCount == 0)
        {
            yield break;
        }

        int targetItem = comesFromCinematic ? script.GetMaxPlane() : script.GetPlane();
        targetItem = Mathf.Clamp(targetItem, 0, itemCount - 1);
        yield return ScrollToItem(targetItem);
    }

    private IEnumerator ScrollToItem(int targetItem)
    {
        isAutoScrolling = true;
        scrollRect.StopMovement();

        float startX = content.localPosition.x;
        float targetX = GetItemPositionX(targetItem);
        float elapsed = 0f;

        while (elapsed < initialScrollDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = initialScrollDuration <= 0f
                ? 1f
                : Mathf.Clamp01(elapsed / initialScrollDuration);
            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);

            content.localPosition = new Vector3(
                Mathf.Lerp(startX, targetX, easedProgress),
                content.localPosition.y,
                content.localPosition.z);
            scrollRect.velocity = Vector2.zero;
            yield return null;
        }

        content.localPosition = new Vector3(
            targetX,
            content.localPosition.y,
            content.localPosition.z);
        scrollRect.StopMovement();

        ItemNumber = targetItem;
        snapSpeed = 0f;
        SetDisplayedName(targetItem);
        isAutoScrolling = false;
    }

    private int GetItemCount()
    {
        int namedItemCount = ItemNames != null ? ItemNames.Length : 0;
        return Mathf.Min(content.childCount, namedItemCount);
    }

    private int GetClosestItemIndex(int itemCount)
    {
        float itemStep = SampleListItem.rect.width + horizontalLayoutGroup.spacing;
        if (itemStep <= Mathf.Epsilon)
        {
            return 0;
        }

        int currentItem = Mathf.RoundToInt(-content.localPosition.x / itemStep);
        return Mathf.Clamp(currentItem, 0, itemCount - 1);
    }

    private float GetItemPositionX(int itemIndex)
    {
        float itemStep = SampleListItem.rect.width + horizontalLayoutGroup.spacing;
        return -itemIndex * itemStep;
    }

    private void SetDisplayedName(int itemIndex)
    {
        if (Name != null && ItemNames != null && itemIndex >= 0 && itemIndex < ItemNames.Length)
        {
            Name.text = ItemNames[itemIndex];
        }
    }
}
