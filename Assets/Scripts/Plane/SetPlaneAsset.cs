using UnityEngine;
using UnityEngine.U2D;

public class SetPlaneAsset : MonoBehaviour
{
    public AnimatorOverrideController overrideControllerTemplate;
    public RuntimeAnimatorController baseController;
    public AnimationClip[] possibleAnimationsActive;
    public Sprite[] planeSprites;

    [SerializeField] SaveStars script;
    [SerializeField] GameObject Plane;

    private void Start()
    {
        Animator animator = Plane.GetComponent<Animator>();
        AnimatorOverrideController instanceOverride = new AnimatorOverrideController(overrideControllerTemplate);

        Plane.GetComponent<SpriteRenderer>().sprite = planeSprites[script.GetPlane()];
        instanceOverride["Plane"] = possibleAnimationsActive[script.GetPlane()];
    }
}
