using UnityEngine;

public class GroundCollide : MonoBehaviour
{
    [SerializeField] private PlaneActions script;
    [SerializeField] private WinCondition script2;
    [SerializeField] private EndLevelMenu script3;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            script.SetGrounded(true);
            script.StopPlane(false);
            switch (script2.GetCondition())
            {
                case true:
                    StartCoroutine(script3.ShowMenuVictory());
                    break;
                case false:
                    StartCoroutine(script3.ShowMenuDefeat());
                    break;
            }
        }
    }
}
