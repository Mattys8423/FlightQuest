using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TypewriterEffectEndCine : MonoBehaviour
{
    public TMP_Text textComponent;
    private string fullText = "Finally, you arrived.\r\nThis is the end of the road.\r\nYou're back home.\r\n\r\nIt took you a long time…\r\nAnd many tries : " + "Exactly.\r\n\r\nHow do I know all this?\r\nBecause I am you.\r\nOr rather…\r\none of your failed attempts.\r\n\r\nThe one who never reached the end.\r\nThe one who never saw home again.";
    public float typingSpeed = 0.05f;
    public int nb = 0;

    [SerializeField] AudioClip son1;
    [SerializeField] AudioClip son2;
    [SerializeField] AudioClip son3;
    [SerializeField] AudioClip son4;
    [SerializeField] AudioClip son5;

    private float fadeDuration = .2f;
    private float delayBeforeFade = 2f;

    public IEnumerator TypeText()
    {
        textComponent.text = "";
        foreach (char c in fullText)
        {
            textComponent.text += c;
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
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(delayBeforeFade);

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);

            Color c = gameObject.GetComponent<TextMeshPro>().color;
            c.a = alpha;
            gameObject.GetComponent<TextMeshPro>().color = c;

            yield return null;
        }
        gameObject.SetActive(false);
        yield return new WaitForSeconds(.2f);
        SceneManager.LoadScene("MenuScene");
    }
}
