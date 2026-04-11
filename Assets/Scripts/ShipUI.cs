using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipUI : MonoBehaviour
{
    private ShipManager shipManager;
    public Slider crewHealthBar;
    public Slider shipHealthBar;
    public Slider crewHungerBar;
    public Slider crewThirstBar;
    public TextMeshProUGUI taskListDisplay;

    private float percentCrewHealth;
    private float percentShipHealth;
    private float percentHunger;
    private float percentThirst;

    TaskManager taskManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shipManager = ShipManager.shipManager;
        taskManager = shipManager.taskManager;
    }

    // Update is called once per frame
    void Update()
    {
        percentCrewHealth = shipManager.crewHealth / 1f;
        percentShipHealth = shipManager.shipHealth / 1f;
        percentHunger = shipManager.crewHunger / 1f;
        percentThirst = shipManager.crewThirst / 1f;

        crewHealthBar.value = percentCrewHealth;
        shipHealthBar.value = percentShipHealth;
        crewHungerBar.value = percentHunger;
        crewThirstBar.value = percentThirst;

        
    }

    public void updateTasksUI() 
    {
        string s = "";
        foreach (Task t in taskManager.activeTasks)
        {
            s += t.name + "\n";
        }
        taskListDisplay.text = s;
    }
}
