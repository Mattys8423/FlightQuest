using System.Collections;
using UnityEngine;

public class InformationsDisplay : MonoBehaviour
{
    [SerializeField] ScrollRectStop script2;
    [SerializeField] GameObject[] TextDescription;

    private bool HasChange;

    // Update is called once per frame
    void Update()
    {
        if (!HasChange)
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
            if (i != script2.PlaneNumber)
            {
                TextDescription[i].SetActive(false);
            }
        }
        yield return new WaitForSeconds(.2f);
        TextDescription[script2.PlaneNumber].SetActive(true);
    }

    public void SetBool(bool value)
    {
        HasChange = value;
    }
}
