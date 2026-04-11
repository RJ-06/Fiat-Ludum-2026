using Unity.Hierarchy;
using UnityEngine;

public class ChangeShipVisibility : MonoBehaviour
{
    public bool viewingTop = true;
    [SerializeField] private GameObject topHalf;
    [SerializeField] private GameObject bottomHalf;
    [SerializeField] private Renderer topRenderer;

    [SerializeField] private float transparentAlpha;
    [SerializeField] private float opaqueAlpha;

    public void ToggleVisibility() 
    {
        viewingTop = !viewingTop;
        if (viewingTop)
        {
            topRenderer.material.color = new Color(topRenderer.material.color.r,
                topRenderer.material.color.g, topRenderer.material.color.b, transparentAlpha);
        }
        else 
        {
            topRenderer.material.color = new Color(topRenderer.material.color.r,
                 topRenderer.material.color.g, topRenderer.material.color.b, opaqueAlpha);
        }
    }
}
