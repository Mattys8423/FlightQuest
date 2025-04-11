using UnityEngine;

public class Lock : MonoBehaviour
{
    bool IsLocked = true;

    public void SetIsLocked(bool isLocked) {  IsLocked = isLocked; }
    public bool GetIsLocked() { return IsLocked; }
}
