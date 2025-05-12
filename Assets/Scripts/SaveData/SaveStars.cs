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

    public void SetPlane(int PlaneNumber)
    {
        starsdata.Plane = PlaneNumber;
        SaveToJson();
    }


    public int GetPlane()
    {
        return starsdata.Plane;
    }

    public void SetBoolTeStar()
    {
        starsdata.hasTenStars = true;
        SaveToJson();
    }


    public bool GetBoolTeStar()
    {
        return starsdata.hasTenStars;
    }

    public void SetBoolTwStar()
    {
        starsdata.hasTenStars = true;
        SaveToJson();
    }


    public bool GetBoolTwStar()
    {
        return starsdata.hasTenStars;
    }

    public void SetBoolThStar()
    {
        starsdata.hasTenStars = true;
        SaveToJson();
    }


    public bool GetBoolThStar()
    {
        return starsdata.hasTenStars;
    }

    public void SetBoolUnlocked()
    {
        starsdata.SecretLevelUnlocked = true;
        SaveToJson();
    }


    public bool GetBoolUnlocked()
    {
        return starsdata.SecretLevelUnlocked;
    }

    public void ResetSave()
    {
        starsdata = new StarData();
        SaveToJson();
        Debug.Log("Reset");
    }

}

[System.Serializable]
public class StarData
{
    public List<LevelStars> levelstar = new List<LevelStars>();
    public int StarsNb;
    public int Plane;
    public bool hasTenStars;
    public bool hasTwentyStars;
    public bool hasThirtyStars;
    public bool SecretLevelUnlocked;
}

[System.Serializable]
public class LevelStars
{
    public string name;
    public int number;
}