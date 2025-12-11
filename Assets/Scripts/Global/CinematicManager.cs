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
        }
    }

    public void DecideMandatory()
    {
        switch (valor)
        {
            case ChoiceValor.Un:
                if (script.GetTotalStars() >= 10 && !script.GetBoolTeStar())
                {
                    MandatoryCine = true;
                    script.SetBoolTeStar();
                }
                break;
            case ChoiceValor.Deux:
                if (script.GetTotalStars() >= 20 && !script.GetBoolTwStar())
                {
                    MandatoryCine = true;
                    script.SetBoolTwStar();
                }
                break;
            case ChoiceValor.Trois:
                if (script.GetTotalStars() >= 30 && !script.GetBoolThStar())
                {
                    MandatoryCine = true;
                    script.SetBoolThStar();
                }
                break;
        }
    }

    public IEnumerator LoadCinematicScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneToLoad);
    }
}
