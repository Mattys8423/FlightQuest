using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndLevelMenu : MonoBehaviour
{
    [SerializeField] private WinCondition script2;
    [SerializeField] private GameObject MenuPause;
    [SerializeField] GameObject Menu;
    [SerializeField] GameObject Star1;
    [SerializeField] GameObject Star2;
    [SerializeField] GameObject Star3;
    [SerializeField] GameObject StarCracked1;
    [SerializeField] GameObject StarCracked2;
    [SerializeField] GameObject StarCracked3;
    [SerializeField] GameObject Win;
    [SerializeField] GameObject Defeat;
    [SerializeField] AudioSource StarsAudio;
    [SerializeField] AudioClip StarWin1;
    [SerializeField] AudioClip StarWin2;
    [SerializeField] AudioClip StarWin3;
    [SerializeField] AudioClip StarsBroken;

    public void MenuReset()
    {
        MenuPause.SetActive(true);
        Menu.SetActive(false);
        Defeat.SetActive(false);
        Win.SetActive(false);
        Star1.GetComponent<Animator>().SetBool("HasStart", false);
        Star2.GetComponent<Animator>().SetBool("HasStart", false);
        Star3.GetComponent<Animator>().SetBool("HasStart", false);
    }

    public IEnumerator ShowMenuDefeat()
    {
        MenuPause.SetActive(false);
        Menu.SetActive(true);
        Defeat.SetActive(true);

        Button NextLevelButton = GameObject.Find("ButtonNextLevel")?.GetComponent<Button>();
        if (NextLevelButton != null)
        {
            NextLevelButton.interactable = false;
        }

        if (script2.GetCoin() == 1)
        {
            yield return new WaitForSeconds(.5f);
            StarCracked1.GetComponent<Animator>().SetBool("HasStart", true);
            yield return new WaitForSeconds(.5f);
            StarsAudio.PlayOneShot(StarsBroken);
        }
        if (script2.GetCoin() == 2)
        {
            yield return new WaitForSeconds(.5f);
            StarCracked1.GetComponent<Animator>().SetBool("HasStart", true);
            yield return new WaitForSeconds(.5f);
            StarsAudio.PlayOneShot(StarsBroken);           
            yield return new WaitForSeconds(.5f);
            StarCracked2.GetComponent<Animator>().SetBool("HasStart", true);
            yield return new WaitForSeconds(.5f);
            StarsAudio.PlayOneShot(StarsBroken);        
        }
        if (script2.GetCoin() == 3)
        {
            yield return new WaitForSeconds(.5f);
            StarCracked1.GetComponent<Animator>().SetBool("HasStart", true);
            yield return new WaitForSeconds(.5f);
            StarsAudio.PlayOneShot(StarsBroken);           
            yield return new WaitForSeconds(.5f);
            StarCracked2.GetComponent<Animator>().SetBool("HasStart", true);
            yield return new WaitForSeconds(.5f);
            StarsAudio.PlayOneShot(StarsBroken);           
            yield return new WaitForSeconds(.5f);
            StarCracked3.GetComponent<Animator>().SetBool("HasStart", true);
            yield return new WaitForSeconds(.5f);
            StarsAudio.PlayOneShot(StarsBroken);        
        }
    }

    public IEnumerator ShowMenuVictory()
    {
        MenuPause.SetActive(false);
        Menu.SetActive(true);
        Win.SetActive(true);
        if (script2.GetCoin() == 1)
        {
            yield return new WaitForSeconds(.5f);
            Star1.GetComponent<Animator>().SetBool("HasStart", true);
            yield return new WaitForSeconds(.5f);
            StarsAudio.PlayOneShot(StarWin1);        
        }
        if (script2.GetCoin() == 2)
        {
            yield return new WaitForSeconds(.5f);
            Star1.GetComponent<Animator>().SetBool("HasStart", true);
            yield return new WaitForSeconds(.5f);
            StarsAudio.PlayOneShot(StarWin1);           
            yield return new WaitForSeconds(.5f);
            Star2.GetComponent<Animator>().SetBool("HasStart", true);
            yield return new WaitForSeconds(.5f);
            StarsAudio.PlayOneShot(StarWin2);        
        }
        if (script2.GetCoin() == 3)
        {
            yield return new WaitForSeconds(.5f);
            Star1.GetComponent<Animator>().SetBool("HasStart", true);
            yield return new WaitForSeconds(.5f);
            StarsAudio.PlayOneShot(StarWin1);           
            yield return new WaitForSeconds(.5f);
            Star2.GetComponent<Animator>().SetBool("HasStart", true);
            yield return new WaitForSeconds(.5f);
            StarsAudio.PlayOneShot(StarWin2);           
            yield return new WaitForSeconds(.5f);
            Star3.GetComponent<Animator>().SetBool("HasStart", true);
            yield return new WaitForSeconds(.5f);
            StarsAudio.PlayOneShot(StarWin3);        
        }

        if (GameObject.FindWithTag("Cinematic"))
        {
            CinematicManager cine = GameObject.FindWithTag("Cinematic").GetComponent<CinematicManager>();
            StartCoroutine(cine.LoadCinematicScene());
        }
        else {
        }
    }
}
