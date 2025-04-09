using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScrollSnapToCenter : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public RectTransform viewport;
    public float snapSpeed = 10f;
    public float snapThreshold = 0.1f;

    private bool isDragging;
    private RectTransform closestItem;

    void Update()
    {
        if (!isDragging)
        {
            SnapToClosestItem();
        }
    }

    void SnapToClosestItem()
    {
        float closestDistance = float.MaxValue;
        RectTransform closestItem = null;

        // Position centrale du viewport EN LOCAL
        Vector3 viewportLocalCenter = viewport.localPosition;

        foreach (RectTransform child in content)
        {
            // Convertir position locale du bouton par rapport au viewport
            Vector3 childLocalPos = content.InverseTransformPoint(child.position);
            float distance = Mathf.Abs(childLocalPos.x);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestItem = child;
            }
        }

        if (closestItem != null)
        {
            // Convertir position cible en local
            Vector3 childLocalPos = content.InverseTransformPoint(closestItem.position);
            Vector3 diff = new Vector3(childLocalPos.x, 0f, 0f);

            Vector3 targetPos = content.localPosition - diff;

            // Snap progressif vers targetPos
            if (Vector3.Distance(content.localPosition, targetPos) > 0.1f)
            {
                content.localPosition = Vector3.Lerp(content.localPosition, targetPos, Time.deltaTime * snapSpeed);
            }
            else
            {
                content.localPosition = targetPos;
            }
        }
    }




    public void OnBeginDrag()
    {
        isDragging = true;
    }

    public void OnEndDrag()
    {
        isDragging = false;
    }
}
