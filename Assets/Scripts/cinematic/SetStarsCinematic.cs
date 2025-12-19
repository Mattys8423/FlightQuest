using UnityEngine;
using UnityEngine.SceneManagement;

public class SetStarsCinematic : MonoBehaviour
{
    [SerializeField] private SaveStars script;

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        
        switch (sceneName)
        {
            case ("NewPlaneCineUn"):
                script.SetBoolTeStar();
                script.SetBoolFromCinematic(true);
                break;
            case ("NewPlaneCineDeux"):
                script.SetBoolTwStar();
                script.SetBoolFromCinematic(true);
                break;
            case ("NewPlaneCineTrois"):
                script.SetBoolThStar();
                script.SetBoolFromCinematic(true);
                break;
        }
    }
}
