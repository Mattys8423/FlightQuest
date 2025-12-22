using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TypewriterEffectEndCine : MonoBehaviour
{
    public TMP_Text textComponent;
    [SerializeField] public SaveStars script;
    private string fullText;
    public float typingSpeed = 0.05f;
    public int nb = 0;

    [SerializeField] AudioClip son1;
    [SerializeField] AudioClip son2;
    [SerializeField] AudioClip son3;
    [SerializeField] AudioClip son4;
    [SerializeField] AudioClip son5;

    private float fadeDuration = 2f;
    private float delayBeforeFade = .5f;
    private bool hasFade = false;

    private void Awake()
    {
        if (script.GetDeaths() == 0)
        {
            fullText = "Finally, you arrived.\r\nThis is the end of the road.\r\nYou’re back home.\r\n\r\nThis time, there were no second try.\r\nNo hesitation.\r\nNo mistakes.\r\n\r\nSo tell me…\r\nDo you still wonder who I am?\r\n\r\nI am what you never became.\r\nA path you didn’t take.\r\nA failure that never existed.\r\n\r\nYou didn’t leave anything behind.\r\nNo echoes.\r\nNo broken versions of yourself.\r\n\r\nYou reached the end.";
        }
        else
        {
            fullText = "Finally, you arrived.\r\nThis is the end of the road.\r\nYou're back home.\r\n\r\nIt took you a long time…\r\nAnd many deaths. " + script.GetDeaths() + " Exactly.\r\n\r\nHow do I know all this?\r\nBecause I am you.\r\nOr rather…\r\none of your failed attempts.\r\n\r\nThe one who never reached the end.\r\nThe one who never saw home again.";
        }
    }

public IEnumerator TypeText()
    {
        textComponent.text = "";

        for (int i = 0; i < fullText.Length; i++)
        {
            char c = fullText[i];
            textComponent.text += c;

            // Détection d’un double saut de ligne (\r\n\r\n)
            if (i >= 3 &&
                fullText[i - 3] == '\r' &&
                fullText[i - 2] == '\n' &&
                fullText[i - 1] == '\r' &&
                fullText[i] == '\n')
            {
                textComponent.text = "";
                yield return new WaitForSeconds(1.2f);
                continue;
            }

            // Pause normale
            yield return new WaitForSeconds(typingSpeed);

            switch (nb)
            {
                case 0:
                    GetComponent<AudioSource>().PlayOneShot(son1);
                    nb = Random.Range(0, 4);
                    break;
                case 1:
                    GetComponent<AudioSource>().PlayOneShot(son2);
                    nb = Random.Range(0, 4);
                    break;
                case 2:
                    GetComponent<AudioSource>().PlayOneShot(son3);
                    nb = Random.Range(0, 4);
                    break;
                case 3:
                    GetComponent<AudioSource>().PlayOneShot(son2);
                    nb = Random.Range(0, 4);
                    break;
                case 4:
                    GetComponent<AudioSource>().PlayOneShot(son3);
                    nb = Random.Range(0, 4);
                    break;
            }
        }
        if (!hasFade)
        {
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        hasFade = true;
        yield return new WaitForSeconds(delayBeforeFade);

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);

            Color c = textComponent.color;
            c.a = alpha;
            textComponent.color = c;

            yield return null;
        }
        yield return new WaitForSeconds(.2f);
        if (script.GetBoolFromCineReviewScene())
        {
            SceneManager.LoadScene("CineReviewScene");
        }
        else
        {
            SceneManager.LoadScene("MenuScene");
        }            
        gameObject.SetActive(false);
    }
}
