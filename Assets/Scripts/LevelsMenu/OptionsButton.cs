using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsButton : MonoBehaviour
{
    [SerializeField] Button MyButton;

    private void Start()
    {
        if (IsLastScene())
        {
            MyButton.interactable = false;
        }
    }

    bool IsLastScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int lastIndex = SceneManager.sceneCountInBuildSettings - 1;

        return currentIndex == lastIndex;
    }
}
