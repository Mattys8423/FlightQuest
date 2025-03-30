using UnityEngine;

public class WinCondition : MonoBehaviour
{
    private bool hasWin = false;
    private int Coin = 0;

    public void SetCondition(bool new_value) {  hasWin = new_value; }
    public bool GetCondition() { return hasWin;}
    public void AddCoin(int Value) {  Coin++; }
    public void RemoveCoin(int Value) { Coin--; }
    public void SetCoin(int Value) { Coin = Value; }
    public int GetCoin() { return Coin; }
}
