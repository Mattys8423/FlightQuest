using UnityEngine;

public class TextGo : MonoBehaviour
{
    [SerializeField] TypewriterEffect script;
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
