using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string description;
    private Sprite sprite;

    public void SetDescription(string desc)
    {
        description = desc;
    }

    public void SetSprite(Sprite spr)
    {
        sprite = spr;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipUI.Instance.Show(description, sprite);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.Hide();
    }
}