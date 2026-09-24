using UnityEngine;

/// <summary>
/// Place this component in Endless-only scenes. Its presence marks the scene
/// as Endless even when it is launched directly from the Unity Editor.
/// </summary>
[AddComponentMenu("FlightQuest/Endless Level Marker")]
[DisallowMultipleComponent]
[DefaultExecutionOrder(2000)]
public sealed class EndlessLevelDetector : MonoBehaviour
{
    [SerializeField, Tooltip("Read-only at runtime: always enabled in a scene containing this marker.")]
    private bool isEndlessMode;

    public bool IsEndlessMode => isEndlessMode;

    private void Awake()
    {
        GameModeState.SelectEndless();
        isEndlessMode = true;
    }

    private void Start()
    {
        DisablePlaneSkills();
        HideSkillInterface();
    }

    public bool GetIsEndlessMode()
    {
        return isEndlessMode;
    }

    private static void DisablePlaneSkills()
    {
        PlaneActions[] planes = Object.FindObjectsByType<PlaneActions>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        foreach (PlaneActions plane in planes)
            plane.SkillNumber = -1;
    }

    private static void HideSkillInterface()
    {
        Transform[] transforms = Object.FindObjectsByType<Transform>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        foreach (Transform candidate in transforms)
        {
            if (candidate.name == "Skill" && candidate.GetComponent<RectTransform>() != null)
                candidate.gameObject.SetActive(false);
        }
    }
}
