using System.Collections;
using UnityEngine;

public class EndLevelMenu : MonoBehaviour
{
    [SerializeField] private WinCondition script2;
    [SerializeField] GameObject Menu;
    [SerializeField] GameObject Star1;
    [SerializeField] GameObject Star2;
    [SerializeField] GameObject Star3;
    [SerializeField] GameObject StarCracked1;
    [SerializeField] GameObject StarCracked2;
    [SerializeField] GameObject StarCracked3;
    [SerializeField] GameObject Win;
    [SerializeField] GameObject Defeat;

    public void MenuReset()
    {
        Menu.SetActive(false);
        Defeat.SetActive(false);
        Win.SetActive(false);
        Star1.GetComponent<Animator>().SetBool("HasStart", false);
        Star2.GetComponent<Animator>().SetBool("HasStart", false);
        Star3.GetComponent<Animator>().SetBool("HasStart", false);
    }

    public IEnumerator ShowMenuDefeat()
    {
        Menu.SetActive(true);
        Defeat.SetActive(true);
        if (script2.GetCoin() == 1)
        {
            yield return new WaitForSeconds(.5f);
            StarCracked1.GetComponent<Animator>().SetBool("HasStart", true);
        }
        if (script2.GetCoin() == 2)
        {
            yield return new WaitForSeconds(.5f);
            StarCracked1.GetComponent<Animator>().SetBool("HasStart", true);
            yield return new WaitForSeconds(.5f);
            StarCracked2.GetComponent<Animator>().SetBool("HasStart", true);
        }
        if (script2.GetCoin() == 3)
        {
            yield return new WaitForSeconds(.5f);
            StarCracked1.GetComponent<Animator>().SetBool("HasStart", true);
            yield return new WaitForSeconds(.5f);
            StarCracked2.GetComponent<Animator>().SetBool("HasStart", true);
            yield return new WaitForSeconds(1f);
            StarCracked3.GetComponent<Animator>().SetBool("HasStart", true);
        }
    }

    public IEnumerator ShowMenuVictory()
    {
        Menu.SetActive(true);
        Win.SetActive(true);
        if (script2.GetCoin() == 1)
        {
            yield return new WaitForSeconds(.5f);
            Star1.GetComponent<Animator>().SetBool("HasStart", true);
        }
        if (script2.GetCoin() == 2)
        {
            yield return new WaitForSeconds(.5f);
            Star1.GetComponent<Animator>().SetBool("HasStart", true);
            yield return new WaitForSeconds(.5f);
            Star2.GetComponent<Animator>().SetBool("HasStart", true);
        }
        if (script2.GetCoin() == 3)
        {
            yield return new WaitForSeconds(.5f);
            Star1.GetComponent<Animator>().SetBool("HasStart", true);
            yield return new WaitForSeconds(.5f);
            Star2.GetComponent<Animator>().SetBool("HasStart", true);
            yield return new WaitForSeconds(1f);
            Star3.GetComponent<Animator>().SetBool("HasStart", true);
        }
    }
}
