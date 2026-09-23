using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Letters follow a clockwise baseline; the ribbon itself can travel either way.
[DisallowMultipleComponent]
[RequireComponent(typeof(Canvas))]
public class Credits : MonoBehaviour
{
    [Header("Texte")]
    [TextArea(3, 12)]
    [SerializeField] private string content = "CRÉDITS   •   GAME DESIGN : Prénom Nom   •   PROGRAMMATION : Prénom Nom   •   GRAPHISMES : Prénom Nom   •   MERCI D'AVOIR JOUÉ !";
    [SerializeField] private TMP_FontAsset font;
    [SerializeField] private Color textColor = Color.red;
    [Tooltip("Taille des lettres en unités de la scène.")]
    [Min(0.01f)] [SerializeField] private float letterSize = 0.65f;

    [Header("Arc autour de la planète")]
    [SerializeField] private Transform planet;
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private Vector2 centerOffset;
    [Min(0.01f)] [SerializeField] private float radius = 7.7f;
    [Tooltip("Extrémité en bas à gauche. 90° = haut ; 180° = gauche.")]
    [Range(90f, 180f)] [SerializeField] private float lowerAngle = 170f;
    [Tooltip("Extrémité près de l'avion, inférieure à Lower Angle.")]
    [Range(0f, 180f)] [SerializeField] private float upperAngle = 92f;

    [Header("Défilement")]
    [Tooltip("Vitesse en unités de la scène par seconde.")]
    [Min(0f)] [SerializeField] private float speed = 0.8f;
    [Tooltip("Coché : de l'avion vers le bas gauche. Décoché : sens inverse.")]
    [SerializeField] private bool towardsLowerLeft = true;
    [SerializeField] private bool loop = true;
    [Min(0f)] [SerializeField] private float gap = 0.5f;
    [Tooltip("Fondu aux deux extrémités, en unités de la scène.")]
    [Min(0f)] [SerializeField] private float fadeDistance = 0.25f;

    [Header("Fin des crédits")]
    [Tooltip("Scène chargée après la sortie complète de la dernière lettre.")]
    [SerializeField] private string sceneAfterCredits = "MenuScene";

    private const float MeshFontSize = 100f;
    private TextMeshProUGUI ribbon;
    private Canvas ownerCanvas;
    private float travelled;
    private string lastContent;
    private bool creditsFinished;
    private bool sceneLoadStarted;

    private float ArcLength => radius * Mathf.Max(1f, lowerAngle - upperAngle) * Mathf.Deg2Rad;

    private void Awake()
    {
        ownerCanvas = GetComponent<Canvas>();
        if (sceneCamera == null)
            sceneCamera = Camera.main;

        if (planet == null || sceneCamera == null)
        {
            Debug.LogError("Credits : renseigne Planet et Scene Camera dans l'Inspector.", this);
            enabled = false;
            return;
        }

        var textObject = new GameObject("CreditsText (arc)", typeof(RectTransform));
        textObject.layer = gameObject.layer;
        textObject.transform.SetParent(transform, false);
        ribbon = textObject.AddComponent<TextMeshProUGUI>();
        RectTransform rect = ribbon.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        if (font != null)
            ribbon.font = font;
        ribbon.fontSize = MeshFontSize;
        ribbon.fontStyle = FontStyles.Bold;
        ribbon.alignment = TextAlignmentOptions.Left;
        ribbon.textWrappingMode = TextWrappingModes.NoWrap;
        ribbon.overflowMode = TextOverflowModes.Overflow;
        ribbon.richText = false;
        ribbon.raycastTarget = false;
        ribbon.OnPreRenderText += BendLetters;
    }

    private void OnEnable()
    {
        travelled = 0f;
        creditsFinished = false;
        sceneLoadStarted = false;
        if (ribbon != null)
            ribbon.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if (ribbon != null)
            ribbon.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (ribbon == null)
            return;
        ribbon.OnPreRenderText -= BendLetters;
        Destroy(ribbon.gameObject);
    }

    private void LateUpdate()
    {
        if (ribbon == null || planet == null || sceneCamera == null)
            return;

        if (creditsFinished)
        {
            LoadSceneAfterCredits();
            return;
        }

        if (lastContent != content)
        {
            // Inspector line breaks become spaces along the same continuous ribbon.
            ribbon.text = (content ?? string.Empty).Replace("\r", " ").Replace("\n", "   ");
            lastContent = content;
            travelled = 0f;
            creditsFinished = false;
        }

        if (font != null && ribbon.font != font)
            ribbon.font = font;
        ribbon.color = textColor;
        travelled += speed * Time.unscaledDeltaTime;
        // Regenerate the straight source mesh before bending, never an already bent mesh.
        ribbon.ForceMeshUpdate();
    }

    private void BendLetters(TMP_TextInfo info)
    {
        if (info.characterCount == 0 || planet == null || sceneCamera == null)
            return;

        float scale = letterSize / MeshFontSize;
        float firstX = info.characterInfo[0].origin;
        float textLength = (info.characterInfo[info.characterCount - 1].xAdvance - firstX) * scale;
        float arcLength = ArcLength;
        float durationDistance = arcLength + textLength + 2f * gap;
        bool reachedEnd = !loop && travelled >= durationDistance;
        travelled = loop
            ? Mathf.Repeat(travelled, durationDistance)
            : Mathf.Min(travelled, durationDistance);
        float offset = towardsLowerLeft
            ? arcLength + gap - travelled
            : -textLength - gap + travelled;

        Vector3 center = planet.position + (Vector3)centerOffset;
        Camera uiCamera = ownerCanvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null : ownerCanvas.worldCamera;

        for (int i = 0; i < info.characterCount; i++)
        {
            TMP_CharacterInfo character = info.characterInfo[i];
            if (!character.isVisible)
                continue;

            Vector3[] vertices = info.meshInfo[character.materialReferenceIndex].vertices;
            Color32[] colors = info.meshInfo[character.materialReferenceIndex].colors32;
            int vertexIndex = character.vertexIndex;
            float middleX = (vertices[vertexIndex].x + vertices[vertexIndex + 2].x) * 0.5f;
            float halfWidth = Mathf.Abs(vertices[vertexIndex + 2].x - vertices[vertexIndex].x) * scale * 0.5f;
            float alongArc = (middleX - firstX) * scale + offset;
            float edgeDistance = Mathf.Min(alongArc - halfWidth, arcLength - alongArc - halfWidth);
            float alpha = edgeDistance < 0f ? 0f
                : fadeDistance > 0f ? Mathf.Clamp01(edgeDistance / fadeDistance) : 1f;

            if (alpha <= 0f)
            {
                for (int corner = 0; corner < 4; corner++)
                {
                    vertices[vertexIndex + corner] = Vector3.zero;
                    colors[vertexIndex + corner].a = 0;
                }
                continue;
            }

            float angle = lowerAngle * Mathf.Deg2Rad - alongArc / radius;
            Vector3 outward = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            Vector3 tangent = new Vector3(outward.y, -outward.x, 0f);
            Vector3 baseline = center + outward * radius;

            for (int corner = 0; corner < 4; corner++)
            {
                int index = vertexIndex + corner;
                Vector3 original = vertices[index];
                Vector3 world = baseline
                    + tangent * ((original.x - middleX) * scale)
                    + outward * ((original.y - character.baseLine) * scale);
                Vector3 screen = sceneCamera.WorldToScreenPoint(world);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    ribbon.rectTransform, screen, uiCamera, out Vector2 local);
                vertices[index] = new Vector3(local.x, local.y, 0f);
                colors[index].a = (byte)(colors[index].a * alpha);
            }
        }

        creditsFinished = reachedEnd;
    }

    private void LoadSceneAfterCredits()
    {
        if (sceneLoadStarted)
            return;

        sceneLoadStarted = true;
        if (string.IsNullOrWhiteSpace(sceneAfterCredits))
        {
            Debug.LogError("Credits : renseigne Scene After Credits dans l'Inspector.", this);
            enabled = false;
            return;
        }

        SceneManager.LoadScene(sceneAfterCredits);
    }

    private void OnValidate()
    {
        radius = Mathf.Max(0.01f, radius);
        letterSize = Mathf.Max(0.01f, letterSize);
        upperAngle = Mathf.Min(upperAngle, lowerAngle - 1f);
        speed = Mathf.Max(0f, speed);
        gap = Mathf.Max(0f, gap);
        fadeDistance = Mathf.Max(0f, fadeDistance);
    }

    private void OnDrawGizmosSelected()
    {
        if (planet == null)
            return;
        Gizmos.color = textColor;
        Vector3 center = planet.position + (Vector3)centerOffset;
        Vector3 previous = Vector3.zero;
        for (int i = 0; i <= 64; i++)
        {
            float angle = Mathf.Lerp(lowerAngle, upperAngle, i / 64f) * Mathf.Deg2Rad;
            Vector3 point = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;
            if (i > 0)
                Gizmos.DrawLine(previous, point);
            previous = point;
        }
    }
}
