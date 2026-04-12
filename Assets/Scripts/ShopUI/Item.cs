using UnityEngine;

public class Item
{
    public enum ItemType
    {
        Oranges,
        Boots,
    }

    public static int GetPrice(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Oranges:
                return 10;
            case ItemType.Boots:
                return 50;
            default:
                return 0;
        }
    }

    public static void DoEffect(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Oranges:
                ShipManager.shipManager.vitaminCLevel += 20;
                break;
            case ItemType.Boots:
                ShipManager.shipManager.speedMultiplier += 0.5f;
                break;
        }
    }
}
