using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class DynamicHorizontalPadding : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public RectTransform firstButton;
    public RectTransform lastButton;

    private HorizontalLayoutGroup layoutGroup;

    void Start()
    {
        layoutGroup = content.GetComponent<HorizontalLayoutGroup>();
        Canvas.ForceUpdateCanvases();
        AdjustPadding();
        scrollRect.horizontalNormalizedPosition = 0f;
    }

    void AdjustPadding()
    {
        float viewportWidth = scrollRect.viewport.rect.width;

        // Largeur des boutons
        float firstWidth = firstButton.rect.width;
        RectTransform penultimate = content.GetChild(content.childCount - 2) as RectTransform;
        float lastWidth = penultimate.rect.width;

        // Calcul padding gauche et droite
        layoutGroup.padding.left = Mathf.RoundToInt(viewportWidth / 2 - firstWidth / 2);
        layoutGroup.padding.right = Mathf.RoundToInt((viewportWidth / 2 - 870) - lastWidth / 2);

        Canvas.ForceUpdateCanvases();
    }
}
