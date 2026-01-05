using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicManager : MonoBehaviour
{
    [SerializeField] private string SceneToLoad;
    [SerializeField] private SaveStars script;
    [SerializeField] private int NumberCine;
    [SerializeField] private bool mandatoryCine;

    private bool hasDecided = false;

    private void Awake()
    {
        mandatoryCine = false;
    }

    public bool DecideMandatory(int addStars)
    {
        if (hasDecided) return mandatoryCine;

        hasDecided = true;

        if (SceneToLoad == "NewPlaneCine")
        {
            int totalStars = script.GetTotalStars() + addStars;
            print(totalStars);

            if (totalStars >= 30 && !script.GetBoolThStar())
            {
                mandatoryCine = true;
                SceneToLoad = "NewPlaneCineTrois";
                return true;
            }

            if (totalStars >= 20 && !script.GetBoolTwStar())
            {
                mandatoryCine = true;
                SceneToLoad = "NewPlaneCineDeux";
                return true;
            }

            if (totalStars >= 10 && !script.GetBoolTeStar())
            {
                mandatoryCine = true;
                SceneToLoad = "NewPlaneCineUn";
                return true;
            }

            mandatoryCine = false;
            return false;
        }

        else
        {
            if (script.GetCine() >= NumberCine)
            {
                mandatoryCine = false;
                return false;
            }

            mandatoryCine = true;
            return true;
        }
    }

    public IEnumerator LoadCinematicScene()
    {
        if (!mandatoryCine)
            yield break;

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneToLoad);
    }
}