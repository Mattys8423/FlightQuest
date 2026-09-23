using UnityEngine;

public class EndStoryMode : MonoBehaviour
{
    [SerializeField] SaveStars script;

    private void Start()
    {
        if (script == null)
        {
            Debug.LogError("EndStoryMode : renseigne la référence SaveStars dans l'Inspector.", this);
            return;
        }

        if (!script.GetBoolStory())
            script.SetBoolStory();
    }
}
