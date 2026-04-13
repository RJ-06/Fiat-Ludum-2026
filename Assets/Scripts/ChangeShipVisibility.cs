using UnityEngine;

public class ChangeShipVisibility : MonoBehaviour
{
    public bool viewingTop = true;
    [SerializeField] private GameObject topHalf;
    [SerializeField] private GameObject bottomHalf;

    [SerializeField] private float transparentAlpha = 0.3f;

    public static ChangeShipVisibility Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Original toggle for the ship's top half
    public void ToggleVisibilityTop()
    {
        viewingTop = !viewingTop;
        SetRendererVisibility(topHalf.gameObject.GetComponent<Renderer>(), viewingTop);
    }

    public void ToggleVisibilityBottom() 
    {
        ToggleVisibility(bottomHalf.gameObject.GetComponent<Renderer>());
    }

    // Overload to toggle visibility for ANY specific renderer
    public void ToggleVisibility(Renderer targetRenderer)
    {
        if (targetRenderer == null) return;

        // Check its current alpha to determine if it's currently transparent
        bool isCurrentlyTransparent = targetRenderer.material.color.a < 1f;

        // If it was transparent, make it opaque (true), otherwise make transparent (false)
        SetRendererVisibility(targetRenderer, isCurrentlyTransparent);
    }

    // Helper method to handle the complex shader changes
    private void SetRendererVisibility(Renderer targetRenderer, bool makeOpaque)
    {
        if (targetRenderer == null) return;

        Material mat = targetRenderer.material;
        Color color = mat.color;

        if (makeOpaque)
        {
            // Revert to Opaque Mode
            mat.SetFloat("_Surface", 0); // 0 = Opaque
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            mat.SetInt("_ZWrite", 1);
            mat.SetInt("_SurfaceType", 0);
            mat.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.SetOverrideTag("RenderType", "Opaque");
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;

            color.a = 1f;
        }
        else
        {
            // Change to Transparent Mode
            mat.SetFloat("_Surface", 1); // 1 = Transparent
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.SetInt("_SurfaceType", 1);
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.SetOverrideTag("RenderType", "Transparent");
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            color.a = transparentAlpha;
        }

        mat.color = color;
    }
}