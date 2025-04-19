using UnityEngine;

public class Hangar : MonoBehaviour
{
    bool HasKey = false;
    bool IsOpen = false;

    // Update is called once per frame
    void Update()
    {
        if (HasKey)
        {
            GetComponent<Animator>().SetTrigger("Open");
            IsOpen = true;
        }
    }

    public void SetBool(bool value) { HasKey = value; }

    public bool GetBool() {  return IsOpen; }
}
