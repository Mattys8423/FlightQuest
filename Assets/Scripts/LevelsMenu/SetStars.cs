using UnityEngine;

public class SetStars : MonoBehaviour
{
    [SerializeField] GameObject Stars1;
    [SerializeField] GameObject Stars2;
    [SerializeField] GameObject Stars3;
    [SerializeField] private SaveStars script4;

    private int NbStars;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string nomParent = transform.parent.name;
        NbStars = script4.GetStars(nomParent);
        switch (NbStars)
        {
            case 0:
                break;
            case 1:
                Stars1.SetActive(true);
                break;
            case 2:
                Stars1.SetActive(true);
                Stars2.SetActive(true);
                break;
            case 3:
                Stars1.SetActive(true);
                Stars2.SetActive(true);
                Stars3.SetActive(true);
                break;
        }
    }
}
