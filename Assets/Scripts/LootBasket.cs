using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class LootBasket : MonoBehaviour
{
    private bool looted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetLooted() => looted;
    public void SetLooted(bool state) => looted = state;
}
