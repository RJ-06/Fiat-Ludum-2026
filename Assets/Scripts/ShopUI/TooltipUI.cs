using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(string description)
    {
        root.SetActive(true);
        text.text = description;
    }

    public void Hide()
    {
        root.SetActive(false);
    }
}