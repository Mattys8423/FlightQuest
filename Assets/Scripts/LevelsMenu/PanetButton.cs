using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class PlanetButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public float clickDelay = 0.3f;  // Temps avant que le clic soit valid�
    private bool isDragging = false;
    private bool pointerDown = false;
    private Coroutine clickCoroutine;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
        isDragging = false;
        clickCoroutine = StartCoroutine(ClickDelayCoroutine());
    }

    public void OnDrag(PointerEventData eventData)
    {
        // D�s que drag est d�tect�, on annule la coroutine de clic
        if (pointerDown)
        {
            isDragging = true;
            if (clickCoroutine != null)
            {
                StopCoroutine(clickCoroutine);
                clickCoroutine = null;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
        if (!isDragging)
        {
            // Le clic est valid�, on lance le niveau
            LaunchLevel();
        }
        // Si drag, ne rien faire, c�est un scroll
    }

    private IEnumerator ClickDelayCoroutine()
    {
        yield return new WaitForSeconds(clickDelay);
        if (!isDragging && pointerDown)
        {
            // L'utilisateur n'a pas drag�, mais maintient le doigt
            LaunchLevel();
            pointerDown = false; // Emp�che de relancer au rel�chement
        }
    }

    private void LaunchLevel()
    {
        Debug.Log("Lancement du niveau !");
        // Ici tu mets ton code pour lancer le niveau
    }
}
