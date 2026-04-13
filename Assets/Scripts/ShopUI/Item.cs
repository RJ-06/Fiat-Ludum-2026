using UnityEngine;

public class Item
{
    public enum ItemType
    {
        Oranges,
        Food, 
        RepairKit,
        Boots,
        ArtOfStarvation,
        ArtOfScurvy,
        RepairManual,
        Cookbook,
        Fishbook,
        Crewmate
    }

    public static Sprite GetSprite(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Oranges:
                return Resources.Load<Sprite>("tradingItems/oranges");
            case ItemType.Boots:
                return Resources.Load<Sprite>("tradingItems/boots");
            case ItemType.Food:
                return Resources.Load<Sprite>("tradingItems/food");
            case ItemType.RepairKit:
                return Resources.Load<Sprite>("tradingItems/repairKit");
            case ItemType.ArtOfStarvation:
                return Resources.Load<Sprite>("tradingItems/starvation");
            case ItemType.ArtOfScurvy:
                return Resources.Load<Sprite>("tradingItems/scurvy");
            case ItemType.RepairManual:
                return Resources.Load<Sprite>("tradingItems/repairManual");
            case ItemType.Cookbook:
                return Resources.Load<Sprite>("tradingItems/cookbook");
            case ItemType.Fishbook:
                return Resources.Load<Sprite>("tradingItems/artOfTheSea");
            case ItemType.Crewmate:
                return Resources.Load<Sprite>("tradingItems/crewmate");
            default:
                return null;
        }
    }

    public static string GetName(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Oranges:
                return "Oranges";
            case ItemType.Food:
                return "Food";
            case ItemType.RepairKit:
                return "Repair Kit";
            case ItemType.Boots:
                return "Boots";
            case ItemType.ArtOfStarvation:
                return "Art of Starvation";
            case ItemType.ArtOfScurvy:
                return "Art of Scurvy";
            case ItemType.RepairManual:
                return "Repair Manual";
            case ItemType.Cookbook:
                return "Art of Cooking";
            case ItemType.Fishbook:
                return "Art of the Sea";
            case ItemType.Crewmate:
                return "Crewmate";
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
            case ItemType.Food:
                return 20;
            case ItemType.RepairKit:
                return 30;
            case ItemType.Boots:
                return 50;
            case ItemType.ArtOfStarvation:
                return 100;
            case ItemType.ArtOfScurvy:
                return 100;
            case ItemType.RepairManual:
                return 150;
            case ItemType.Cookbook:
                return 200;
            case ItemType.Fishbook:
                return 200;
            case ItemType.Crewmate:
                return 500;
            default:
                return 0;
        }
    }

    public static string GetDescription(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Oranges:
                return "Increases vitamin C level by 20.";
            case ItemType.Food:
                return "Increases hunger level by 20.";
            case ItemType.RepairKit:
                return "Increases ship health by 20.";
            case ItemType.Boots:
                return "Increases your speed by 50%.";
            case ItemType.ArtOfStarvation:
                return "Increases hunger decrease multiplier by 20% (lose hunger slower).";
            case ItemType.ArtOfScurvy:
                return "Increases vitamin C decrease multiplier by 20%.";
            case ItemType.RepairManual:
                return "Increases repair efficiency multiplier by 20% (fix things faster).";
            case ItemType.Cookbook:
                return "Increases cooking level by 1. Gain more food from cooking minigame.";
            case ItemType.Fishbook:
                return "Increases fishing level by 1. Gain more loot from fishing minigame.";
            case ItemType.Crewmate:
                return "Adds a crew member to your ship to help do tasks. When standing near an issue, press (Q) to summon the crewmate to repair it.";
            default:
                return "";
        }
    }

    public static void DoEffect(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Oranges:
                ShipManager.shipManager.vitaminCLevel += 20;
                break;
            case ItemType.Food:
                ShipManager.shipManager.crewHunger += 10;
                break;
            case ItemType.RepairKit:
                ShipManager.shipManager.shipHealth += 20;
                break;
            case ItemType.Boots:
                ShipManager.shipManager.speedMultiplier += 0.5f;
                break;
            case ItemType.ArtOfStarvation:
                ShipManager.shipManager.hungerDecreaseMultiplier += 0.2f;
                break;
            case ItemType.ArtOfScurvy:
                ShipManager.shipManager.vitaminCDecreaseMultiplier += 0.2f;
                break;
            case ItemType.RepairManual:
                ShipManager.shipManager.repairEfficiencyMultiplier += 0.2f;
                break;
            case ItemType.Cookbook:
                ShipManager.shipManager.cookingLevel += 1;
                break;
            case ItemType.Fishbook:
                ShipManager.shipManager.fishingLevel += 1;
                break;
            case ItemType.Crewmate:
                ShipManager.shipManager.numCrew += 1;
                break;
        }
    }
}
