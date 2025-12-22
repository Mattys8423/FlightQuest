using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicManager : MonoBehaviour
{
    [SerializeField] public string SceneToLoad;
    [SerializeField] SaveStars script;
    [SerializeField] public bool MandatoryCine = false;

    private void Start()
    {
        if (SceneToLoad != "NewPlaneCine")
        {
            MandatoryCine = true;
        }
    }

    public bool DecideMandatory(int add_coin)
    {
        if (SceneToLoad != "NewPlaneCine") return true;
        switch (script.GetTotalStars() + add_coin)
        {
            case (>= 30):
                if (script.GetBoolThStar() == false)
                {
                    return true;
                }
                break;
            case (>= 20):
                if (script.GetBoolTwStar() == false)
                {
                    return true;
                }
                break;
            case (>= 10):
                if (script.GetBoolTeStar() == false)
                {
                    return true;                    
                }
                break;
        }
        return false;
    }

    public IEnumerator LoadCinematicScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneToLoad);
    }
}
