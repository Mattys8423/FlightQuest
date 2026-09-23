using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Empêche les boutons placés dans une ScrollRect de valider un clic
// lorsque le geste de l'utilisateur servait à faire défiler le contenu.
[RequireComponent(typeof(ScrollRect))]
public sealed class ScrollClickGuard : MonoBehaviour,
    IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Tooltip("Une pression pendant que la liste glisse encore ne lance pas de niveau.")]
    [SerializeField, Min(0f)] private float movingVelocityThreshold = 80f;

    private static int activeDrags;
    private static int suppressUntilFrame = -1;

    private ScrollRect scrollRect;
    private bool dragging;

    public static bool ShouldSuppressClick =>
        activeDrags > 0 || Time.frameCount <= suppressUntilFrame;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetState()
    {
        activeDrags = 0;
        suppressUntilFrame = -1;
    }

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        // Un toucher destiné à arrêter l'inertie de la liste ne doit pas ouvrir
        // le niveau qui se trouve momentanément sous le doigt.
        if (scrollRect.velocity.sqrMagnitude > movingVelocityThreshold * movingVelocityThreshold)
            SuppressForRelease();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!dragging)
        {
            dragging = true;
            activeDrags++;
        }

        SuppressForRelease();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StopTrackingDrag();
        SuppressForRelease();
    }

    private void OnDisable()
    {
        StopTrackingDrag();
    }

    private void StopTrackingDrag()
    {
        if (!dragging)
            return;

        dragging = false;
        activeDrags = Mathf.Max(0, activeDrags - 1);
    }

    private static void SuppressForRelease()
    {
        // Le Button.onClick est envoyé après OnEndDrag. On conserve donc le
        // verrou pendant l'image courante et la suivante.
        suppressUntilFrame = Mathf.Max(suppressUntilFrame, Time.frameCount + 1);
    }
}
