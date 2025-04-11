using UnityEngine;
using UnityEngine.UI;

public class SetLock : MonoBehaviour
{
    [SerializeField] SaveStars script;
    [SerializeField] ScrollRectStop script2;
    [SerializeField] GameObject Plane1;
    [SerializeField] GameObject Plane2;
    [SerializeField] GameObject Plane3;
    [SerializeField] GameObject Plane4;
    bool hasAnimate = false;

    private void Start()
    {
        int totalStars = script.GetTotalStars();
        Plane1.GetComponent<Lock>().SetIsLocked(false);

        if (totalStars > 10)
        {
            Plane2.transform.Find("Locket").GetComponent<Image>().enabled = false;
            Plane2.GetComponent<Lock>().SetIsLocked(false);
        }
        if (totalStars > 20)
        {
            Plane3.transform.Find("Locket").GetComponent<Image>().enabled = true;
            Plane3.GetComponent<Lock>().SetIsLocked(false);
        }
        if (totalStars > 30)
        {
            Plane4.transform.Find("Locket").GetComponent<Image>().enabled = true;
            Plane4.GetComponent<Lock>().SetIsLocked(false);
        }
    }

    private void Update()
    {
        int totalStars = script.GetTotalStars();
        if (totalStars >= 10)
        {
            if (totalStars < 20 && script2.PlaneNumber == 1 && !hasAnimate)
            {
                Animator locket = Plane2.transform.Find("Locket").GetComponent<Animator>();
                locket.SetTrigger("Unlock");
                hasAnimate = true;
            }
        }

        if (totalStars >= 20)
        {
            Plane3.GetComponent<Lock>().SetIsLocked(false);

            if (totalStars < 30 && script2.PlaneNumber == 2 && !hasAnimate)
            {
                Animator locket = Plane2.transform.Find("Locket").GetComponent<Animator>();
                locket.SetTrigger("Unlock");
                hasAnimate = true;
            }
        }

        if (totalStars >= 30)
        {
            Plane4.GetComponent<Lock>().SetIsLocked(false);

            if (totalStars < 40 && script2.PlaneNumber == 3 && !hasAnimate)
            {
                Animator locket = Plane2.transform.Find("Locket").GetComponent<Animator>();
                locket.SetTrigger("Unlock");
                hasAnimate = true;
            }
        }
    }
}
