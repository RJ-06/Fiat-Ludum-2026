using UnityEngine;

public class ChangeShipVisibility : MonoBehaviour
{
    public bool viewingTop = true;
    [SerializeField] private GameObject topHalf;
    [SerializeField] private GameObject bottomHalf;
    [SerializeField] private Renderer topRenderer;


    [SerializeField] private Material transparentMat;
    [SerializeField] private Material opaqueMat;

    public static ChangeShipVisibility Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void ToggleVisibility()
    {
        viewingTop = !viewingTop;
        Color color = topRenderer.material.color;

        if (viewingTop)
        {
            topRenderer.material = opaqueMat;
        }
        else
        {
            topRenderer.material = transparentMat;
        }
    }
}