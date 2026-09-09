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
            fullText = "Finally, you arrived.\r\nThis is home.\r\n\r\nYou crossed every planet without falling once.\r\nNo crash.\r\nNo second try.\r\nNo long chain of mistakes to learn from.\r\n\r\nNot everyone reaches their goal so quickly.\r\nSome need time.\r\nSome need failure.\r\nSome need to lose their way before they understand the road.\r\n\r\nBut you found it on your first flight.\r\n\r\nA long and difficult path stood between you and home...\r\nand somehow,\r\nyou made it look almost simple.\r\n\r\nWelcome home.";
        }
        else
        {
            fullText = "Finally, you arrived.\r\nThis is home.\r\n\r\nYou fell " + script.GetDeaths() + " times before reaching this sky.\r\n" +
                       script.GetDeaths() + " attempts.\r\n" +
                       script.GetDeaths() + " lessons written across the stars.\r\n\r\n" +
                       "But that is how a goal is reached.\r\n" +
                       "Not all at once.\r\n" +
                       "Not without doubt.\r\n" +
                       "Not without falling short again and again.\r\n\r\n" +
                       "The road home was long.\r\n" +
                       "It was slow.\r\n" +
                       "It was difficult.\r\n\r\n" +
                       "And still, you kept trying.\r\n\r\n" +
                       "Every mistake brought you closer.\r\n" +
                       "Every failure taught you where not to fall.\r\n" +
                       "Every attempt carried you a little farther.\r\n\r\n" +
                       "You did not arrive because the journey was easy.\r\n" +
                       "You arrived because you refused to stop.\r\n\r\n" +
                       "Welcome home.";
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
