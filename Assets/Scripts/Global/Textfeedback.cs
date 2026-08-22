using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class Textfeedback : MonoBehaviour
{
    [SerializeField] float ScaleMultiplier;
    [SerializeField] float AnimDuration;
    [SerializeField] int Choose;

    [SerializeField] private PlaneActions script;
    
    private TMP_Text text;
    private Vector3 Scale;

    private void Start()
    {
        text = gameObject.GetComponent<TMP_Text>();
        Scale = text.gameObject.transform.localScale;
        StartCoroutine(FeedBack());
    }
    
    private IEnumerator FeedBack()
    {
        yield return new WaitForSeconds(3f);
        switch (Choose) 
        { 
            case 0:
                while(script.NumberOfLaunch > 0) 
                {
                    StartCoroutine(ScaleAnimation());
                }
                break;
            case 1:
                while (!script.GetDJ())
                {
                    StartCoroutine(ScaleAnimation());
                }
                break;
            default:
                break;
        }
    }

    private IEnumerator ScaleAnimation()
    {
        Vector3 targetScale = Scale * ScaleMultiplier;

        // Agrandissement
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / AnimDuration;
            text.transform.localScale = Vector3.Lerp(Scale, targetScale, t);
            yield return null;
        }

        // Réduction
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / AnimDuration;
            text.transform.localScale = Vector3.Lerp(targetScale, Scale, t);
            yield return null;
        }

        text.transform.localScale = Scale;
    }
}
