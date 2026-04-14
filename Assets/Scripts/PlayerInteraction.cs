using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    private bool nearFoodLoot = false, nearMoneyLoot = false, startedLooting = false;
    private GameObject lootableObject;
    public TMPro.TMP_Text text;

    int foodLooted = 0;
    int goldLooted = 0;


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
                FindAnyObjectByType<TextFade>().Show("Gained 20 food!");

                ShipManager.shipManager.crewHunger = Mathf.Min(100f, ShipManager.shipManager.crewHunger + 20f);
                foodLooted += 20;
            }
            else if (nearMoneyLoot)
            {

                float rawMoneyAmount = Random.Range(30f, 200f);
                int adjustedMoney = (int)Mathf.Round(Mathf.Pow(1.02604f, rawMoneyAmount) + 29f);
                FindAnyObjectByType<TextFade>().Show("Gained " + adjustedMoney + " gold!");

                ShipManager.shipManager.gold += adjustedMoney;
                goldLooted += adjustedMoney;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            text.text = "The townspeople caught you stealing! You lost 250 gold.";
            Time.timeScale = 0f;
            ShipManager.shipManager.gold = Mathf.Max(0, ShipManager.shipManager.gold - 250);
            StartCoroutine(SwitchSceneAfterDelay());
        }
    }

    private void Update()
    {
        if (transform.position.y < -10f)
        {
            text.text = "You successfully escaped with " + goldLooted + " gold and " + foodLooted + " food.";
            Time.timeScale = 0f;
            StartCoroutine(SwitchSceneAfterDelay());
        }
    }

    IEnumerator SwitchSceneAfterDelay()
    {
        yield return new WaitForSecondsRealtime(5f);
        SwitchScene();
    }
    void SwitchScene()
    {
        Time.timeScale = 1f;
        ShipManager.shipManager.sceneIndex++;
        string nextScene = ShipManager.shipManager.sceneList[ShipManager.shipManager.sceneIndex];
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
    }
}