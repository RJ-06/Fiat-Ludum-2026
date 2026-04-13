using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public static ShipManager shipManager;
    public float crewHunger = 100;
    public float vitaminCLevel = 20;
    public float shipHealth = 100;

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
    public int gold;

    public TaskManager taskManager;

    public int sceneIndex = 0;
    public List<string> sceneList = new List<string>() {"TutorialLevel", "TradingScene", "Level2", "TradingScene" };


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

    public void TakeDamage(float damage)
    {
        shipHealth -= damage;
        if (shipHealth <= 0f)
        {
            shipHealth = 0f;
            // Handle ship destruction or game over logic here
            Debug.Log("Ship destroyed! Game Over.");
        }
    }
}
