using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public void ChangeFood(int amount)
    {
        ShipManager.shipManager.crewHunger += amount;
        ShipManager.shipManager.crewHunger = Mathf.Clamp(ShipManager.shipManager.crewHunger, 0, 100);
    }

    public void ChangeMoney(int amount)
    {
        ShipManager.shipManager.gold += amount;
    }
}
