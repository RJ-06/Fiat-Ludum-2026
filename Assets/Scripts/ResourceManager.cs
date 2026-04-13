using Mono.Cecil;
using UnityEngine;

public struct Resources
{
    public int food;
    public int money;

    public Resources(int m_food, int m_money)
    {
        food = m_food;
        money = m_money;
    }
}

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
