using System.Collections.Generic;
using UnityEngine;
using static GameInstance;

public class GameInstance : MonoBehaviour
{
    private Dictionary<string, int> starsPerLevel = new Dictionary<string, int>();

    public static GameInstance instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        starsPerLevel["Level1"] = 0;
        starsPerLevel["Level2"] = 0;
        starsPerLevel["Level3"] = 0;
    }

    public void SetStars(string level, int Stars)
    {
        if (starsPerLevel.ContainsKey(level))
        {
            if (starsPerLevel[level] > Stars) {}
            else if (starsPerLevel[level] < Stars) {starsPerLevel[level] = Stars;}
            Debug.Log(level + " a maintenant " + starsPerLevel[level] + " étoiles.");
        }
        else
        {
            Debug.LogWarning("Le niveau spécifié n'existe pas !");
        }
    }

    public int GetStars(string level)
    {
        if (starsPerLevel.ContainsKey(level))
        {
            Debug.Log(starsPerLevel[level] + " étoiles.");
            return starsPerLevel[level];
        }
        else
        {
            Debug.LogWarning("Le niveau spécifié n'existe pas !");
            return 0;
        }
    }
}
