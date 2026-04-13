using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIShop : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;

    List<Item.ItemType> availableItems;

    [SerializeField] AudioSource buySuccessSound;
    [SerializeField] AudioSource buyFailureSound;


    private void Awake()
    {
        container = transform.Find("Container");
        shopItemTemplate = container.Find("ShopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }

    public static List<T> GetRandomElements<T>(int count) where T : Enum
    {
        // 1. Get all values from the enum
        T[] allValues = (T[])Enum.GetValues(typeof(T));

        // 2. Shuffle using a simple Fisher-Yates or LINQ (if performance allows)
        // Fisher-Yates is better for performance, but LINQ is concise:
        return allValues.OrderBy(x => UnityEngine.Random.value)
                        .Take(count)
                        .ToList();
    }

    void Start()
    {
        availableItems = GetRandomElements<Item.ItemType>(8);
        CreateShop(availableItems);
    }

    public void CreateShop(List<Item.ItemType> availableItems)
    {
        foreach (Transform child in container)
        {
            if (child != shopItemTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < availableItems.Count; i++)
        {
            Item.ItemType itemType = availableItems[i];
            CreateItemButton(itemType, Item.GetSprite(itemType), Item.GetName(itemType), Item.GetPrice(itemType), i);
        }
    }

    private void CreateItemButton(Item.ItemType itemType, Sprite itemSprite, string itemName, int itemCost, int positionIndex)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        shopItemTransform.gameObject.SetActive(true);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = 40;
        shopItemRectTransform.anchoredPosition = new Vector2(0, -shopItemHeight * positionIndex);

        //shopItemRectTransform.Find("Icon").GetComponent<UnityEngine.UI.Image>().sprite = itemSprite;
        shopItemRectTransform.Find("Name").GetComponent<TMP_Text>().text = itemName;
        shopItemRectTransform.Find("Price").GetComponent<TMP_Text>().text = itemCost.ToString();

        var hover = shopItemTransform.GetComponent<ShopItemHover>();
        hover.SetDescription(Item.GetDescription(itemType));
        hover.SetSprite(itemSprite);

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
            buySuccessSound.Play();
            Debug.Log($"Bought {itemType}");
        }
        else
        {
            buyFailureSound.Play();
            Debug.Log($"Not enough money to buy {itemType}");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShipManager.shipManager.sceneIndex++;
            SceneManager.LoadScene(ShipManager.shipManager.sceneList[ShipManager.shipManager.sceneIndex]);
        }
    }
}
