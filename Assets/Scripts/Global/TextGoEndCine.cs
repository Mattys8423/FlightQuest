using UnityEngine;

public class TextGoEndCine : MonoBehaviour
{
    [SerializeField] TypewriterEffectEndCine script;
    [SerializeField] public bool CanBeActivated = false;
    bool HasBeenActivated = false;

    // Update is called once per frame
    void Update()
    {
        if (!HasBeenActivated && CanBeActivated)
        {
            StartCoroutine(script.TypeText());
            HasBeenActivated = true;
        }
    }
}
