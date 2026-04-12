using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public void OpenShop()
    {
        UIShop uiShop = FindAnyObjectByType<UIShop>();
        uiShop.gameObject.SetActive(true);
        uiShop.CreateShop(new System.Collections.Generic.List<Item.ItemType>
        {
            Item.ItemType.Oranges,
            Item.ItemType.Boots
        });
    }


}
