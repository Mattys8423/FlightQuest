using UnityEngine;

public class PlayTimeManager : MonoBehaviour
{
    public static PlayTimeManager Instance;

    public float TotalPlayTime { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadTime();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        TotalPlayTime += Time.unscaledDeltaTime;
    }

    void OnApplicationPause(bool pause)
    {
        if (pause) SaveTime();
    }

    void OnApplicationQuit()
    {
        SaveTime();
    }

    void SaveTime()
    {
        PlayerPrefs.SetFloat("TotalPlayTime", TotalPlayTime);
        PlayerPrefs.Save();
    }

    void LoadTime()
    {
        TotalPlayTime = PlayerPrefs.GetFloat("TotalPlayTime", 0f);
    }

    public int GetFormattedTimeHours()
    {
        int hours = Mathf.FloorToInt(TotalPlayTime / 3600);
        return hours;
    }

    public int GetFormattedTimeMin()
    {
        int minutes = Mathf.FloorToInt((TotalPlayTime % 60) / 60);
        return minutes;
    }
}
