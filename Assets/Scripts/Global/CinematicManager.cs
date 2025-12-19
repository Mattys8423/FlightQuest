using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicManager : MonoBehaviour
{
    [SerializeField] public string SceneToLoad;
    [SerializeField] SaveStars script;
    [SerializeField] public bool MandatoryCine = false;
    public enum ChoiceValor
    {
        Un = 1,
        Deux = 2,
        Trois = 3
    }
    [SerializeField] private ChoiceValor valor = ChoiceValor.Un;

    private void Start()
    {
        if (SceneToLoad != "NewPlaneCine")
        {
            MandatoryCine = true;
        }
        else {
            SceneToLoad = "NewPlaneCine"+valor.ToString();
        }
    }

    public bool DecideMandatory(int add_coin)
    {
        if (SceneToLoad != "NewPlaneCine" + valor.ToString()) return true;
        switch (valor)
        {
            case ChoiceValor.Un:
                if (script.GetTotalStars() + add_coin >= 10 && script.GetBoolTeStar() == false)
                {
                    return true;
                }
                break;
            case ChoiceValor.Deux:
                if (script.GetTotalStars() + add_coin >= 20 && script.GetBoolTwStar() == false)
                {
                    return true;
                }
                break;
            case ChoiceValor.Trois:
                if (script.GetTotalStars() + add_coin >= 30 && script.GetBoolThStar() == false)
                {
                    return true;                    
                }
                break;
        }
        return false;
    }

    public IEnumerator LoadCinematicScene()
    {
        script.SetBoolFromCinematic(true);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneToLoad);
    }
}
