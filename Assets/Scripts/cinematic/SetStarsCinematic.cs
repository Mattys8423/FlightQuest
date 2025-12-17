using UnityEngine;
using UnityEngine.SceneManagement;

public class SetStarsCinematicx : MonoBehaviour
{
    [SerializeField] private SaveStars script;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        
        switch (sceneName)
        {
            case ("NewPlaneCineUn"):
                script.SetBoolTeStar();
                break;
            case ("NewPlaneCineDeux"):
                script.SetBoolTwStar();
                break;
            case ("NewPlaneCineTrois"):
                script.SetBoolThStar();
                break;
        }
    }
}
