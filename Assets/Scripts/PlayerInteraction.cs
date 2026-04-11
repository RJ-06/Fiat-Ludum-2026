using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] ResourceManager resourceManager;
    private bool nearFoodLoot = false, nearMoneyLoot = false, startedLooting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnInteractPress(InputValue value)
    {
        if (nearFoodLoot || nearMoneyLoot) startedLooting = true;
    }

    private void OnInteractRelease(InputValue value)
    {
        if (startedLooting)
        {
            startedLooting = false;
            if (nearFoodLoot)
            {
                ++resourceManager.resources.food;
                Debug.Log("Food is now " + resourceManager.resources.food);
            }
            else if (nearMoneyLoot)
            {
                resourceManager.resources.money += Random.Range(30f, 200f);
                Debug.Log("Money is now " + resourceManager.resources.money);
            }            
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag[..4] == "Loot")
        {
            Debug.Log("Can collect loot");
            if (other.gameObject.tag[4..] == "Food") nearFoodLoot = true;
            else if (other.gameObject.tag[4..] == "Money") nearMoneyLoot = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag[..4] == "Loot")
        {
            nearFoodLoot = false;
            nearMoneyLoot = false;
        }
    }
}
