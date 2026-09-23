using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField code;
    [SerializeField] private Menu_actions script;

    public void OnEnter()
    {
        switch (code.text)
        {
            case "princesse23/08":
                script.playlevel("CamilleLevel");
                break;
        }
    }
}
