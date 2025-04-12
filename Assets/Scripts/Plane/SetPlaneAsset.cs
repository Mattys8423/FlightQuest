using UnityEngine;
using UnityEngine.U2D;

public class SetPlaneAsset : MonoBehaviour
{
    public RuntimeAnimatorController[] baseController;
    public Sprite[] planeSprites;

    [SerializeField] SaveStars script;
    [SerializeField] GameObject Plane;

    private void Start()
    {
        Animator animator = Plane.GetComponent<Animator>();
        animator.runtimeAnimatorController = baseController[script.GetPlane()];
        Plane.GetComponent<SpriteRenderer>().sprite = planeSprites[script.GetPlane()];
    }
}
