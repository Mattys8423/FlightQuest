using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;

public class PlayLevel : MonoBehaviour
{
    [SerializeField] SaveStars script;
    private bool LevelPass = false;
    private int LevelCount;
    private string LevelName;

    private void Start()
    {
        string fullName = gameObject.name;

        Match match = Regex.Match(name, @"Level(\d+)");
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
    }

    public void playlevel()
    {
        StartCoroutine(WaitAndPlayName(gameObject.name));
    }

    public bool GetLevelPass()
    {
        return LevelPass;
    }
    public int GetLevelCount()
    {
        return LevelCount;
    }
    public string GetLevelName()
    {
        return LevelName;
    }

    IEnumerator WaitAndPlayName(string SceneName)
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneName);
    }
}
