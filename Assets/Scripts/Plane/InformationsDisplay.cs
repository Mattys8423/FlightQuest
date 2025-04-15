using System.Collections;
using UnityEngine;

public class InformationsDisplay : MonoBehaviour
{
    [SerializeField] ScrollRectStop script2;
    [SerializeField] GameObject[] TextDescription;

    // Update is called once per frame
    void Update()
    {
        switch (script2.PlaneNumber)
        {
            case 0:
                StartCoroutine(ShowText());
                break;
            case 1:
                StartCoroutine(ShowText());
                break;
            case 2:
                StartCoroutine(ShowText());
                break;
            case 3:
                StartCoroutine(ShowText());
                break;
        }
    }

    private IEnumerator ShowText()
    {
        for (int i = 0; i < TextDescription.Length; i++)
        {
            TextDescription[i].SetActive(false);
        }
        yield return new WaitForSeconds(1f);
        TextDescription[script2.PlaneNumber].SetActive(true);
    }
}
