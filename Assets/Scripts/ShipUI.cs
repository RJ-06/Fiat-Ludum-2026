using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipUI : MonoBehaviour
{
    private ShipManager shipManager;
    public Slider crewHealthBar;
    public Slider shipHealthBar;

    private float percentCrewHealth;
    private float percentShipHealth;
    private float percentHunger;
    private float percentThirst;

    private TaskManager taskManager;

    public GameObject imagePrefab; // UI Image prefab
    private RectTransform canvas;   // Your Canvas

    void Start()
    {
        shipManager = ShipManager.shipManager;
        taskManager = TaskManager.taskManagerSingleton;
        canvas = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //updateTasksUI();
        //percentCrewHealth = shipManager.crewHealth / 1f;
        //percentShipHealth = shipManager.shipHealth / 1f;
        //percentHunger = shipManager.crewHunger / 1f;
        //percentThirst = shipManager.crewThirst / 1f;

        //crewHealthBar.value = percentCrewHealth;
        //shipHealthBar.value = percentShipHealth;
        //crewHungerBar.value = percentHunger;
        //crewThirstBar.value = percentThirst;
    }
}
