using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public static ShipManager shipManager;
    public float crewHunger;
    public float vitaminCLevel;
    public float shipHealth;

    public float hungerDecreaseRate;
    public float vitaminCDecreaseRate;

    [Header("Player Stats")]
    public float speedMultiplier = 1f;
    public int cookingLevel = 1;
    public int fishingLevel = 1;
    public int numCrew = 0;
    public float repairEfficiencyMultiplier = 1f;
    public float crewEfficiencyMultiplier = 1f;
    public float hungerDecreaseMultiplier = 1f;
    public float vitaminCDecreaseMultiplier = 1f;

    [Header("Resources")]
    public int gold = 100;

    public TaskManager taskManager;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (shipManager != null && shipManager != this)
        {
            Destroy(gameObject); // kill duplicates
            return;
        }

        shipManager = this;
        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        crewHunger -= hungerDecreaseRate * hungerDecreaseMultiplier;
        vitaminCLevel -= vitaminCDecreaseRate * vitaminCDecreaseMultiplier;
    }
}
