using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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
    public int cookingLevel = 0;
    public int fishingLevel = 0;
    public int numCrew = 0;
    public float repairEfficiencyMultiplier = 1f;
    public float hungerDecreaseMultiplier = 1f;
    public float vitaminCDecreaseMultiplier = 1f;

    public float actingSpeedMultiplier = 1f;
    public float actingEfficiencyMultiplier = 1f;
    public float actingHungerDecreaseMultiplier = 1f;

    [Header("Resources")]
    public int gold = 10000;

    public Queue<CrewmateBehavior> crewmateQueue = new Queue<CrewmateBehavior>();
    public GameObject crewmate;

    public int sceneIndex = 0;
    public List<string> sceneList = new List<string>() { "TutorialLevel", "TradingScene", "Level2", "IslandRaid", "TradingScene", "Level3", "TradingScene", "KrakenFight" };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (shipManager != null && shipManager != this)
        {
            Destroy(gameObject);
            return;
        }

        shipManager = this;
        DontDestroyOnLoad(gameObject);
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (scene.name.Contains("Level"))
        {
            crewmateQueue.Clear();

            for (int i = 0; i < numCrew; i++)
            {
                GameObject newCrew = Instantiate(crewmate, new Vector3(-1, 8, 12), Quaternion.identity);
                crewmateQueue.Enqueue(newCrew.GetComponent<CrewmateBehavior>());
            }
        }
    }

    private void FixedUpdate()
    {
        if (sceneList[sceneIndex].Contains("Level"))
        {

            crewHunger -= hungerDecreaseRate * hungerDecreaseMultiplier * actingHungerDecreaseMultiplier * Time.deltaTime;

            if (crewHunger < 30f)
            {
                actingEfficiencyMultiplier = 0.5f;
                actingSpeedMultiplier = 0.5f;
            }
            else
            {
                actingEfficiencyMultiplier = 1f;
                actingSpeedMultiplier = 1f;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        shipHealth -= damage;
        if (shipHealth <= 0f)
        {
            shipHealth = 0f;
            // Handle ship destruction or game over logic here
            Debug.Log("Ship destroyed! Game Over.");
            UnityEngine.SceneManagement.SceneManager.LoadScene("DeathScene");
        }
    }

    public void AddCrew()
    {
        numCrew++;
    }
}