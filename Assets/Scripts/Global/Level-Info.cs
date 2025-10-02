using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInfo : MonoBehaviour
{
    [SerializeField] SaveStars script;
    [SerializeField] TMP_Text textComponent;
    private bool LevelPass = false;
    private int LevelCount;
    private string LevelName;

    private void Start()
    {
        string fullName = SceneManager.GetActiveScene().name;

        Match match = Regex.Match(fullName, @"Level(\d+)");
        if (match.Success)
        {
            LevelCount = int.Parse(match.Groups[1].Value);
        }

        Match match2 = Regex.Match(fullName, @"^Level\d+");
        if (match2.Success)
        {
            LevelName = match2.Value;
        }

        if (script.GetStars(LevelName) > 0)
        {
            LevelPass = true;
        }

        textComponent.text = LevelCount.ToString();
    }

    public bool GetLevelPass()
    {
        return LevelPass;
    }
}
