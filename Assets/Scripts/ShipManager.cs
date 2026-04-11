using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public static ShipManager shipManager;
    public float crewHungerBar;
    public float crewThirstBar;
    public float crewHealth;
    public float shipHealth;

    public float hungerDecreaseRate;
    public float thirstDecreaseRate;
    public float healthDecrease;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shipManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        crewHungerBar -= hungerDecreaseRate;
        crewThirstBar -= thirstDecreaseRate;
        if (crewHungerBar <= 0) 
        {
            crewHealth -= healthDecrease;
        }
        if (crewThirstBar <= 0) 
        {
            crewHealth -= healthDecrease;
        }

    }
}
