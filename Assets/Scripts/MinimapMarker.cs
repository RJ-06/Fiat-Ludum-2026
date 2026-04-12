using UnityEngine;

public class MinimapMarker : MonoBehaviour
{
    private RectTransform icon;
    private Camera minimapCamera;
    private RectTransform minimapRect;

    public bool isCautionMarker;

    void Start()
    {
        minimapCamera = MinimapManager.Instance.minimapCamera;
        minimapRect = MinimapManager.Instance.minimapRect;

        if (isCautionMarker)
        {
            icon = Instantiate(
                MinimapManager.Instance.markerCautionPrefab,
                minimapRect
            );
        }
        else
            icon = Instantiate(
            MinimapManager.Instance.markerPrefab,
            minimapRect
        );
    }

    void Update()
    {
        Vector3 viewportPos = minimapCamera.WorldToViewportPoint(transform.position);

        icon.anchoredPosition = new Vector2(
            (viewportPos.x - 0.5f) * minimapRect.sizeDelta.x,
            (viewportPos.y - 0.5f) * minimapRect.sizeDelta.y
        );
    }

    void OnDestroy()
    {
        if (icon != null)
            Destroy(icon.gameObject);
    }
}