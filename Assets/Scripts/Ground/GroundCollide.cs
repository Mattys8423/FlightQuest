using UnityEngine;

public class GroundCollide : MonoBehaviour
{
    [SerializeField] private PlaneActions script;
    [SerializeField] private WinCondition script2;
    [SerializeField] private EndLevelMenu script3;
    [SerializeField] private SaveStars script4;

    public string LevelName;

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
                    script4.SetStars(LevelName, script2.GetCoin());
                    break;
                case false:
                    StartCoroutine(script3.ShowMenuDefeat());
                    script4.SetStars(LevelName, 0);
                    break;
            }
        }
    }
}
