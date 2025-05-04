using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public TMP_Text textComponent;
    public string fullText;
    public float typingSpeed = 0.05f;
    public int nb = 0;

    [SerializeField] AudioClip son1;
    [SerializeField] AudioClip son2;
    [SerializeField] AudioClip son3;
    [SerializeField] AudioClip son4;
    [SerializeField] AudioClip son5;
    [SerializeField] GameObject Button;

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
        yield return new WaitForSeconds(.2f);
        Button.SetActive(true);
    }
}
