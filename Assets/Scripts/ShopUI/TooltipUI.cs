using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text text;
    [SerializeField] private UnityEngine.UI.Image image;

    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(string description, Sprite itemSprite)
    {
        root.SetActive(true);
        text.text = description;
        image.sprite = itemSprite;
    }

    public void Hide()
    {
        text.text = "Description";
    }
}