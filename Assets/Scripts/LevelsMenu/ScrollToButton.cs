using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ScrollToButton : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public float duration = 0.5f;
    private RectTransform targetButton;


    private void Start()
    {
        StartCoroutine(FindButtonLevelAndScroll());
    }

    public void CenterButtonAtIndex(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= content.childCount) return;

        targetButton = content.GetChild(levelIndex) as RectTransform;
        StartCoroutine(SmoothScrollTo());
    }

    IEnumerator SmoothScrollTo()
    {
        yield return null; // attendre que le layout soit bien calculé

        float elapsedTime = 0f;

        float contentWidth = content.rect.width;
        float viewportWidth = scrollRect.viewport.rect.width;

        // Position X du bouton dans le content
        float targetX = Mathf.Abs(targetButton.anchoredPosition.x);

        // Position normalisée cible (0 = gauche, 1 = droite)
        float targetNormalizedPos =
            Mathf.Clamp01((targetX - viewportWidth / 2f) / (contentWidth - viewportWidth));

        float startNormalizedPos = scrollRect.horizontalNormalizedPosition;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            scrollRect.horizontalNormalizedPosition =
                Mathf.Lerp(startNormalizedPos, targetNormalizedPos, Mathf.SmoothStep(0f, 1f, t));

            yield return null;
        }

        scrollRect.horizontalNormalizedPosition = targetNormalizedPos;
    }

    IEnumerator FindButtonLevelAndScroll()
    {
        yield return new WaitForSeconds(.2f);

        Button[] buttons = content.GetComponentsInChildren<Button>()
            .Where(b => b.gameObject.name != "GalaxyEasterEgg")
            .ToArray();

        // On trie les boutons selon leur numéro de niveau
        var sortedButtons = buttons
            .OrderBy(b => b.GetComponent<PlayLevel>().GetLevelCount())
            .ToList();

        int lastUnlockedIndex = 0;

        for (int i = 0; i < sortedButtons.Count; i++)
        {
            if (i == 0) continue;

            bool previousPassed = sortedButtons[i - 1].GetComponent<PlayLevel>().GetLevelPass();
            if (previousPassed)
            {
                lastUnlockedIndex = i; // On garde en mémoire le dernier débloqué
            }
            else
            {
                break;
            }
        }

        CenterButtonAtIndex(lastUnlockedIndex);
    }
}
