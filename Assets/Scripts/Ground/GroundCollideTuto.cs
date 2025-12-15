using System.Collections;
using UnityEngine;

public class GroundCollideTuto : MonoBehaviour
{
    [SerializeField] private PlaneActions script;
    [SerializeField] private WinCondition script2;
    [SerializeField] private EndLevelMenu script3;
    [SerializeField] private SaveStars script4;

    private void OnTriggerExit2D(Collider2D collision)
    {
        this.gameObject.GetComponent<PolygonCollider2D>().isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            script.SetGrounded(true);
            script.StopPlane(false);
            if (script.NumberOfLaunch > 0 ) {}
            else
            {
                StartCoroutine(WaitForTutoEnd());
            }
            this.gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
        }
    }

    IEnumerator WaitForTutoEnd()
    {
        yield return new WaitForSeconds(2);
        switch (script2.GetCondition())
        {
            case true:
                StartCoroutine(script3.ShowMenuVictory());
                script4.SetStars("Level1", script2.GetCoin());
                break;
            case false:
                StartCoroutine(script3.ShowMenuDefeat());
                script4.SetStars("Level1", 0);
                script4.AddDeath();
                break;
        }
    }
}
