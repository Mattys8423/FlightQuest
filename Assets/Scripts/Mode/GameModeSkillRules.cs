using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum FlightQuestGameMode
{
    Story,
    Endless
}

/// <summary>
/// Keeps the selected mode while navigating between menus and levels.
/// This state is deliberately separate from the existing gameplay scripts.
/// </summary>
public static class GameModeState
{
    public static FlightQuestGameMode Current { get; private set; } = FlightQuestGameMode.Story;

    public static void SelectStory()
    {
        Current = FlightQuestGameMode.Story;
    }

    public static void SelectEndless()
    {
        Current = FlightQuestGameMode.Endless;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetState()
    {
        Current = FlightQuestGameMode.Story;
    }
}

/// <summary>
/// Connects the mode buttons and installs the skill rule without requiring
/// changes to PlaneActions or to every individual level scene.
/// </summary>
public static class GameModeSkillBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        SceneManager.sceneLoaded -= ConfigureScene;
        SceneManager.sceneLoaded += ConfigureScene;
        ConfigureScene(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private static void ConfigureScene(Scene scene, LoadSceneMode loadMode)
    {
        if (scene.name == "ModeScene")
            ConnectModeButtons();

        // This also makes Play Mode testing work when the Endless menu scene
        // is launched directly instead of being reached through ModeScene.
        if (scene.name == "EndlessMode" || scene.name == "EndlessModeScene")
            GameModeState.SelectEndless();

        PlaneActions[] planes = Object.FindObjectsByType<PlaneActions>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        foreach (PlaneActions plane in planes)
        {
            if (!plane.TryGetComponent(out PlaneSkillModeRule _))
                plane.gameObject.AddComponent<PlaneSkillModeRule>();
        }

        if (planes.Length > 0 && GameModeState.Current == FlightQuestGameMode.Endless)
            HideSkillInterface();
    }

    private static void ConnectModeButtons()
    {
        ConnectButton("ButtonStory", GameModeState.SelectStory);
        ConnectButton("ButtonEndless", GameModeState.SelectEndless);
    }

    private static void ConnectButton(string objectName, UnityEngine.Events.UnityAction action)
    {
        GameObject buttonObject = GameObject.Find(objectName);
        if (buttonObject == null || !buttonObject.TryGetComponent(out Button button))
            return;

        button.onClick.RemoveListener(action);
        button.onClick.AddListener(action);
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

/// <summary>
/// Runs after PlaneActions.Start, which normally selects the plane skill.
/// An unsupported skill number keeps all flight and launch logic intact while
/// making PlaneActions.SpecialSkill perform no action in Endless mode.
/// </summary>
[DefaultExecutionOrder(1000)]
internal sealed class PlaneSkillModeRule : MonoBehaviour
{
    private PlaneActions plane;

    private void Awake()
    {
        plane = GetComponent<PlaneActions>();
    }

    private void Start()
    {
        if (GameModeState.Current == FlightQuestGameMode.Endless)
            plane.SkillNumber = -1;
    }
}
