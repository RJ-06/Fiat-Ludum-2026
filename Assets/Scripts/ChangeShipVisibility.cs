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
        Debug.Log("toggling visibility");
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

    private void OnTriggerEnter(Collider other) //IF YOU'RE ON BOTTOM
    {
        if (other.GetComponent<PlayerMovement>() != null && viewingTop) 
        {
            ToggleVisibility();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null && !viewingTop)
        {
            ToggleVisibility();
        }
    }
}
