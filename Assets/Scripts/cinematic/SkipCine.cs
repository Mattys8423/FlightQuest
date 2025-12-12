using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkipCine : MonoBehaviour
{
    [SerializeField] private string SceneToLoad;
    [SerializeField] public Slider holdSlider;
    private bool Hold = false;
    private bool Skipped = false;
    private bool State = true;
    private float HoldTime = 0f;
    private float DontHoldTime= 0f;

    // Update is called once per frame
    void Update()
    {
        holdSlider.value = HoldTime;

        if (Input.GetMouseButton(0))
        {
            Hold = true;
        }

        if (Hold)
        {
            if (!State)
            {
                Show();
            }
            DontHoldTime = 0;
            HoldTime += Time.deltaTime;
        }
        else 
        { 
            HoldTime = 0;
            DontHoldTime += Time.deltaTime;
        }

        if (HoldTime > 2 && !Skipped) 
        {
            SceneManager.LoadScene(SceneToLoad);
            Skipped = true;
        }

        if (DontHoldTime > 1 && State)
        {
            Hide();
        }
    }

    private void Show()
    {
        GetComponent<Animator>().Play("apparition");
        State = true;
    }

    private void Hide()
    {
        GetComponent<Animator>().Play("disparition");
        State = false;
    }
}
