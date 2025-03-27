using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puff : MonoBehaviour
{
    [SerializeField] GameObject PuffImg;
    [SerializeField] GameObject Plane;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    void Start()
    {
        initialPosition = this.gameObject.transform.position;
        initialRotation = this.gameObject.transform.rotation;
        initialScale = this.gameObject.transform.localScale;
    }

    public void PlacePuff()
    {
        initialPosition = Plane.transform.position;
        initialRotation = Plane.transform.rotation;
        initialScale = Plane.transform.localScale;
    }

    public IEnumerator LaunchPuff()
    {
        this.gameObject.transform.position = initialPosition;
        this.gameObject.transform.rotation = initialRotation;
        this.gameObject.transform.localScale = initialScale;
        PuffImg.SetActive(true);
        this.gameObject.GetComponent<Animator>().Play("Puff");
        yield return new WaitForSeconds(.3f);
        PuffImg.SetActive(false);
    }
}

