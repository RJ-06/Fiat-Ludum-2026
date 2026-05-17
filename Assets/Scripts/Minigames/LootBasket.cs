using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class LootBasket : MonoBehaviour
{
    private bool looted = false;
    public bool GetLooted() => looted;
    public void SetLooted(bool state) => looted = state;
}
