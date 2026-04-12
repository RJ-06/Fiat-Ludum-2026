using UnityEngine;

public class ChangeShipVisibility : MonoBehaviour
{
    public bool viewingTop = true;
    [SerializeField] private GameObject topHalf;
    [SerializeField] private GameObject bottomHalf;
    [SerializeField] private Renderer topRenderer;


    [SerializeField] private Material transparentMat;
    [SerializeField] private Material opaqueMat;

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

    private void OnTriggerEnter(Collider other) //IF YOU'RE ON BOTTOM
    {
        Debug.Log("going to loweer deck");
        if (other.GetComponent<PlayerMovement>() != null && viewingTop)
        {
            ToggleVisibility();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("going to uper deck");
        if (other.GetComponent<PlayerMovement>() != null && !viewingTop)
        {
            ToggleVisibility();
        }
    }
}