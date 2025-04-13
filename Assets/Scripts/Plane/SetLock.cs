using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SetLock : MonoBehaviour
{
    [SerializeField] SaveStars script;
    [SerializeField] ScrollRectStop script2;
    [SerializeField] AudioSource Effect;
    [SerializeField] AudioClip Unlock;

    [SerializeField] List<GameObject> planes;

    bool hasAnimate = false;

    private void Start()
    {
        int totalStars = script.GetTotalStars();
        planes[0].GetComponent<Lock>().SetIsLocked(false);

        for (int i = 1; i < planes.Count; i++)
        {
            if (totalStars > 10 * (i + 1))
            {
                GameObject locket = planes[i].transform.Find("Locket")?.gameObject;
                locket.SetActive(false);
                planes[i].GetComponent<Lock>().SetIsLocked(false);
            }
        }
    }

    private void Update()
    {
        int totalStars = script.GetTotalStars();

        for (int i = 1; i < planes.Count; i++)
        {
            if (totalStars >= 10 * i)
            {
                planes[i].GetComponent<Lock>().SetIsLocked(false);

                if (totalStars < 10 * (i + 1) && script2.PlaneNumber == i && !hasAnimate)
                {
                    Animator locket = planes[i].transform.Find("Locket")?.GetComponent<Animator>();
                    if (locket != null)
                    {
                        Effect.PlayOneShot(Unlock);
                        locket.SetTrigger("Unlock");
                        hasAnimate = true;
                    }
                }
            }
        }
    }
}
