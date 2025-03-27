using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPause : MonoBehaviour
{
    [SerializeField] private InputActionReference inputActions;
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject Player;
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
            Player.GetComponent<PlaneActions>().enabled = false;
        }
        else
        {
            Player.GetComponent<PlaneActions>().enabled = true;
        }
    }

    public void CrossPause()
    {
        Menu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
        Player.GetComponent<PlaneActions>().enabled = true;
    }
}