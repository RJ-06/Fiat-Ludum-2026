using UnityEngine;

public class Item
{
    public enum ItemType
    {
        Oranges,
        Boots,
    }

    //public static Sprite GetSprite(ItemType itemType)
    //{
    //    switch (itemType)
    //    {
    //        case ItemType.Oranges:
    //            return Resources.Load<Sprite>("Sprites/Shop/oranges");
    //        case ItemType.Boots:
    //            return Resources.Load<Sprite>("Sprites/Shop/boots");
    //        default:
    //            return null;
    //    }
    //}

    public static string GetName(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Oranges:
                return "Oranges";
            case ItemType.Boots:
                return "Boots";
            default:
                return "";
        }
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
