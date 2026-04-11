using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] ResourceManager resourceManager;
    private bool nearLoot = false, startedLooting = false;

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
        if (nearLoot) startedLooting = true;
    }

    private void OnInteractRelease(InputValue value)
    {
        if (startedLooting)
        {
            startedLooting = false;
            ++resourceManager.resources.food;
            Debug.Log("Food is now " + resourceManager.resources.food);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Loot"))
        {
            Debug.Log("Can collect loot");
            nearLoot = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Loot"))
        {
            nearLoot = false;
        }
    }
}
