using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipUI : MonoBehaviour
{
    private ShipManager shipManager;
    public Slider crewHungerBar;
    public Slider shipHealthBar;

    private float percentShipHealth;
    private float percentHunger;

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
        percentShipHealth = shipManager.shipHealth / 100f;
        percentHunger = shipManager.crewHunger / 100f;

        crewHungerBar.value = percentHunger;
        shipHealthBar.value = percentShipHealth;
    }
}
