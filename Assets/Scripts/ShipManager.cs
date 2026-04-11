using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public static ShipManager shipManager;
    public float crewHunger;
    public float crewThirst;
    public float crewHealth;
    public float shipHealth;

    public float hungerDecreaseRate;
    public float thirstDecreaseRate;
    public float healthDecrease;

    public TaskManager taskManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shipManager = this;
        taskManager = GetComponent<TaskManager>();
        taskManager.AddTask();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        crewHunger -= hungerDecreaseRate;
        crewThirst -= thirstDecreaseRate;
        if (crewHunger <= 0) 
        {
            crewHealth -= healthDecrease;
        }
        if (crewThirst <= 0) 
        {
            crewHealth -= healthDecrease;
        }

    }
}
