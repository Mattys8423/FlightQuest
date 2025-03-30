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
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, data);
        Debug.Log("Sauvegarde réussi");
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
                SaveToJson();
            }
            Debug.Log(level + " a maintenant " + levelEntry.number + " étoiles.");
        }
        else
        {
            levelEntry = new LevelStars { name = level, number = Stars };
            starsdata.levelstar.Add(levelEntry);
            SaveToJson();
            Debug.Log(level + " a maintenant " + levelEntry.number + " étoiles.");
        }
    }

    public int GetStars(string level)
    {
        level = level.Trim();
        LevelStars levelEntry = starsdata.levelstar.Find(entry => entry.name == level);

        if (levelEntry != null)
        {
            Debug.Log(levelEntry.number + " étoiles.");
            return levelEntry.number;
        }
        else
        {
            Debug.Log("Le niveau " + level + " n'existe pas");
            return 0;
        }
    }

}

[System.Serializable]
public class StarData
{
    public List<LevelStars> levelstar = new List<LevelStars>();
}

[System.Serializable]
public class LevelStars
{
    public string name;
    public int number;
}