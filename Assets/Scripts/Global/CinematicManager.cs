using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicManager : MonoBehaviour
{
    [SerializeField] private string SceneToLoad;
    [SerializeField] private SaveStars script;
    [SerializeField] private int NumberCine;

    private bool mandatoryCine;

    private void Awake()
    {
        if (script.GetCine() >= NumberCine)
        {
            enabled = false;
            return;
        }

        mandatoryCine = SceneToLoad != "NewPlaneCine";
    }

    public bool DecideMandatory(int add_coin)
    {
        if (!enabled)
            return false;

        if (SceneToLoad != "NewPlaneCine")
            return true;

        int totalStars = script.GetTotalStars() + add_coin;

        if (totalStars >= 30 && !script.GetBoolThStar())
            return true;

        if (totalStars >= 20 && !script.GetBoolTwStar())
            return true;

        if (totalStars >= 10 && !script.GetBoolTeStar())
            return true;

        return false;
    }

    public IEnumerator LoadCinematicScene()
    {
        if (!enabled)
            yield break;

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneToLoad);
    }
}
