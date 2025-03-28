using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPause : MonoBehaviour
{
    [SerializeField] private InputActionReference inputActions;
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject PlaneGame;
    private bool isPaused = false;

    private void Awake()
    {
        inputActions.action.performed += DisplayPause;
    }

    private void OnEnable()
    {
        inputActions.action.Enable();
    }

    private void OnDisable()
    {
        inputActions.action.Disable();
    }

    private void OnDestroy()
    {
        inputActions.action.performed -= DisplayPause;
    }

    private void DisplayPause(InputAction.CallbackContext context)
    {
        Menu.SetActive(!Menu.activeSelf);
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (Menu.activeSelf)
        {
            PlaneGame.GetComponent<PlaneActions>().enabled = false;
        }
        else
        {
            PlaneGame.GetComponent<PlaneActions>().enabled = true;
        }
    }

    public void DisplayPause()
    {
        Menu.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
        PlaneGame.GetComponent<PlaneActions>().enabled = false;
    }

    public void CrossPause()
    {
        Menu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
        PlaneGame.GetComponent<PlaneActions>().enabled = true;
    }
}