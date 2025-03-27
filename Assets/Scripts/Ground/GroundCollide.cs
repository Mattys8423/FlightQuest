using UnityEngine;

public class GroundCollide : MonoBehaviour
{
    [SerializeField] private PlaneActions script;
    [SerializeField] private WinCondition script2;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            script.StopPlane();
            switch (script2.GetCondition())
            {
                case true:

                    break;
                case false:
                    break;
            }
        }
    }
}
