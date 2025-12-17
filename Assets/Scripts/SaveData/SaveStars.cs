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


    public void AddCine()
    {
        starsdata.NbCinematic += 1;
        SaveToJson();
    }

    public int GetCine()
    {
        return starsdata.NbCinematic;
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
        starsdata.hasTwentyStars = true;
        SaveToJson();
    }


    public bool GetBoolTwStar()
    {
        return starsdata.hasTwentyStars;
    }

    public void SetBoolThStar()
    {
        starsdata.hasThirtyStars = true;
        SaveToJson();
    }


    public bool GetBoolThStar()
    {
        return starsdata.hasThirtyStars;
    }

    public void SetBoolCineSpace()
    {
        starsdata.CineSpace = true;
        SaveToJson();
    }


    public bool GetBoolCineSpace()
    {
        return starsdata.CineSpace;
    }

    public void SetBoolCineFalseEnd()
    {
        starsdata.CineFalseEnd = true;
        SaveToJson();
    }


    public bool GetBoolCineFalseEnd()
    {
        return starsdata.CineFalseEnd;
    }

    public int GetMaxPlane()
    {
        if (GetBoolTeStar())
        {
            return 1;
        }
        if (GetBoolTwStar())
        {
            return 2;
        }
        if (GetBoolThStar())
        {
            return 3;
        }
        else
        {
            return 0;
        }
    }

    public void AddDeath()
    {
        starsdata.Deaths += 1;
        SaveToJson();
    }

    public int GetDeaths()
    {
        return starsdata.Deaths;
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

    public void SetBoolInversed(bool boolean)
    {
        starsdata.IsInversed = boolean;
        SaveToJson();
    }


    public bool GetBoolInversed()
    {
        return starsdata.IsInversed;
    }

    public void SetBoolFromCinematic(bool boolean)
    {
        starsdata.FromCinematic = boolean;
        SaveToJson();
    }


    public bool GetBoolFromCinematic()
    {
        return starsdata.FromCinematic;
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
    public int Deaths;
    public int NbCinematic;
    public bool hasTenStars;
    public bool CineSpace;
    public bool CineFalseEnd;
    public bool hasTwentyStars;
    public bool hasThirtyStars;
    public bool SecretLevelUnlocked;
    public bool IsInversed = false;
    public bool FromCinematic = false;
}

[System.Serializable]
public class LevelStars
{
    public string name;
    public int number;
}