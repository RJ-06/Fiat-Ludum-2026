using Mono.Cecil;
using UnityEngine;

public struct Resources
{
    public int food;
    public int water;
    public float money;

    public Resources(int m_food, int m_water, float m_money)
    {
        food = m_food;
        water = m_water;
        money = m_money;
    }
}

public class ResourceManager : MonoBehaviour
{
    public Resources resources = new(30, 30, 500f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int ChangeFood(int amount)
    {
        if (resources.food + amount >= 0) resources.food += amount;
        else Debug.Log("Error: negative food");
        return resources.food;
    }

    public int ChangeWater(int amount)
    {
        if (resources.water + amount >= 0) resources.water += amount;
        else Debug.Log("Error: negative water");
        return resources.water;
    }

    public float ChangeMoney(float amount)
    {
        resources.money += amount;
        if (resources.money <= 0f) Debug.Log("We're in debt chat");
        return resources.money;
    }
}
