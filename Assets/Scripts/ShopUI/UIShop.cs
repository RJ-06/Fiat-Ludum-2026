using UnityEngine;
using TMPro;

public class UIShop : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;

    private void Awake()
    {
        container = transform.Find("Container");
        shopItemTemplate = container.Find("ShopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        CreateItemButton(Item.ItemType.Oranges, null, "Oranges", 10, 0);
        CreateItemButton(Item.ItemType.Boots, null, "Boots", 50, 1);
    }

    private void CreateItemButton(Item.ItemType itemType, Sprite itemSprite, string itemName, int itemCost, int positionIndex)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        shopItemTransform.gameObject.SetActive(true);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = 40;
        shopItemRectTransform.anchoredPosition = new Vector2(0, -shopItemHeight * positionIndex);

        //shopItemRectTransform.Find("itemImage").GetComponent<UnityEngine.UI.Image>().sprite = itemSprite;
        shopItemRectTransform.Find("Name").GetComponent<TMP_Text>().text = itemName;
        shopItemRectTransform.Find("Price").GetComponent<TMP_Text>().text = itemCost.ToString();

        shopItemRectTransform
            .GetComponent<UnityEngine.UI.Button>()
            .onClick.AddListener(() => TryToBuy(itemType));
    }

    private void TryToBuy(Item.ItemType itemType) 
    {
        if (ShipManager.shipManager.gold >= Item.GetPrice(itemType))
        {
            ShipManager.shipManager.gold -= Item.GetPrice(itemType);
            Item.DoEffect(itemType);
            Debug.Log($"Bought {itemType}");
        }
        else
        {
            Debug.Log($"Not enough money to buy {itemType}");
        }
    }
}
