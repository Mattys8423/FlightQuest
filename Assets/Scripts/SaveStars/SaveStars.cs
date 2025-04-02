using System.Collections.Generic;
using UnityEngine;

public class SaveStars : MonoBehaviour
{
    public StarData starsdata = new StarData();

    private void Awake()
    {
        LoadFromJson();
    }

    public void SaveToJson()
    {
        string data = JsonUtility.ToJson(starsdata);
        string filePath = Application.persistentDataPath + "/StarsData.json";
        System.IO.File.WriteAllText(filePath, data);
    }

    public void LoadFromJson()
    {
        string filePath = Application.persistentDataPath + "/StarsData.json";
        string data = System.IO.File.ReadAllText(filePath);

        starsdata = JsonUtility.FromJson<StarData>(data);
    }

    public void SetStars(string level, int Stars)
    {
        level = level.Trim();
        LevelStars levelEntry = starsdata.levelstar.Find(entry => entry.name == level);

        if (levelEntry != null)
        {
            if (levelEntry.number < Stars)
            {
                levelEntry.number = Stars;
                starsdata.StarsNb += Stars;
                SaveToJson();
            }
        }
        else
        {
            levelEntry = new LevelStars { name = level, number = Stars };
            starsdata.levelstar.Add(levelEntry);
            starsdata.StarsNb += Stars;
            SaveToJson();
        }
    }

    public int GetStars(string level)
    {
        level = level.Trim();
        LevelStars levelEntry = starsdata.levelstar.Find(entry => entry.name == level);

        if (levelEntry != null)
        {
            return levelEntry.number;
        }
        else
        {
            return 0;
        }
    }

    public void SetTotalStars()
    {
        starsdata.StarsNb = 0;
        foreach (LevelStars levelEntry in starsdata.levelstar)
        {
            starsdata.StarsNb += levelEntry.number;
        }
        SaveToJson();
    }


    public int GetTotalStars()
    {
        return starsdata.StarsNb;
    }

}

[System.Serializable]
public class StarData
{
    public List<LevelStars> levelstar = new List<LevelStars>();
    public int StarsNb;
}

[System.Serializable]
public class LevelStars
{
    public string name;
    public int number;
}