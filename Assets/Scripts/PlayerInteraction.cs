using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] ResourceManager resourceManager;
    private bool nearFoodLoot = false, nearMoneyLoot = false, startedLooting = false;
    private GameObject lootableObject;

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
        if ((nearFoodLoot || nearMoneyLoot) && !lootableObject.GetComponent<LootBasket>().GetLooted()) startedLooting = true;
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
                float rawMoneyAmount = Random.Range(30f, 200f);
                int adjustedMoney = (int)Mathf.Round(Mathf.Pow(1.02604f, rawMoneyAmount) + 29f);
                resourceManager.resources.money += adjustedMoney;
                Debug.Log("Money is now " + resourceManager.resources.money);
            }

            // Make sure basket can't be looted more than once
            if (lootableObject != null) lootableObject.GetComponent<LootBasket>().SetLooted(true);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag[..4] == "Loot")
        {
            Debug.Log("Can collect loot");
            lootableObject = other.gameObject;
            if (other.gameObject.tag[4..] == "Food") nearFoodLoot = true;
            else if (other.gameObject.tag[4..] == "Money") nearMoneyLoot = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag[..4] == "Loot")
        {
            lootableObject = null;
            nearFoodLoot = false;
            nearMoneyLoot = false;
        }
    }
}
