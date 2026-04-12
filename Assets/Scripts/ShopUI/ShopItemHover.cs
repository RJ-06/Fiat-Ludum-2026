using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string description;

    public void SetDescription(string desc)
    {
        description = desc;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipUI.Instance.Show(description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.Hide();
    }
}