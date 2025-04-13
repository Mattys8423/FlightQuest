using UnityEngine;

public class SetStarsTuto : MonoBehaviour
{
    [SerializeField] GameObject Stars1;
    [SerializeField] GameObject Stars2;
    [SerializeField] GameObject Stars3;
    [SerializeField] private SaveStars script4;
    [SerializeField] private string Level;

    private int NbStars;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string nomParent = transform.parent.name;
        NbStars = script4.GetStars(Level);
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
