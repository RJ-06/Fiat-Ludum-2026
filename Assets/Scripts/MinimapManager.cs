using UnityEngine;

public class MinimapManager : MonoBehaviour
{
    public static MinimapManager Instance;
    public Camera minimapCamera;
    public RectTransform minimapRect;
    public RectTransform markerPrefab;
    public RectTransform markerCautionPrefab;

    void Awake()
    {
        Instance = this;
    }
}