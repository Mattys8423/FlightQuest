using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class DynamicHorizontalPadding : MonoBehaviour
{
    public ScrollRect scrollRect;           // Scroll View
    public RectTransform content;           // Content du Scroll
    public RectTransform firstButton;       // Premier bouton
    public RectTransform lastButton;        // Dernier bouton
    public float duration;

    private HorizontalLayoutGroup layoutGroup;

    void Start()
    {
        layoutGroup = content.GetComponent<HorizontalLayoutGroup>();
        Canvas.ForceUpdateCanvases();
        AdjustPadding();
        StartCoroutine(SmoothScrollToEnd());
    }

    void AdjustPadding()
    {
        float viewportWidth = scrollRect.viewport.rect.width;

        // Largeur des boutons
        float firstWidth = firstButton.rect.width;
        float lastWidth = lastButton.rect.width;

        // Calcul padding gauche et droite
        layoutGroup.padding.left = Mathf.RoundToInt(viewportWidth / 2 - firstWidth / 2);
        layoutGroup.padding.right = Mathf.RoundToInt(viewportWidth / 2 - lastWidth / 2);

        Canvas.ForceUpdateCanvases();  // Recalculer après changement de padding
    }

    IEnumerator SmoothScrollToEnd()
    {
        Canvas.ForceUpdateCanvases();
        float elapsed = 0f;
        float start = scrollRect.horizontalNormalizedPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(start, 0f, elapsed / duration);
            yield return null;
        }
    }

}
