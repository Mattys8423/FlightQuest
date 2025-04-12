using UnityEngine;
using UnityEngine.EventSystems;

public class Click : MonoBehaviour
{
    public void CloseMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);

        //var button = gameObject.GetComponent<UnityEngine.UI.Button>();
        //if (button != null)
        //{
        //    button.OnDeselect(null);
        //}
    }
}
