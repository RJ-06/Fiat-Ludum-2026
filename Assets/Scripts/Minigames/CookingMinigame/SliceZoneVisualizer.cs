using UnityEngine;
using System.Collections.Generic;

public class SliceZoneVisualizer : MonoBehaviour
{
    [SerializeField] private RectTransform zonePrefab;
    [SerializeField] private RectTransform markerPrefab;
    [SerializeField] private RectTransform parent;

    private List<GameObject> markers = new List<GameObject>();

    public void DrawZones(List<SliceZone> zones)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        markers.Clear();

        foreach (var zone in zones)
        {
            RectTransform obj = Instantiate(zonePrefab, parent);
            RectTransform rect = obj.GetComponent<RectTransform>();

            rect.anchoredPosition = new Vector2(zone.position, 0);
            rect.sizeDelta = new Vector2(zone.radius * 2f, rect.sizeDelta.y);
        }
    }

    public void DrawSliceMarker(float position)
    {
        RectTransform obj = Instantiate(markerPrefab, parent);
        RectTransform rect = obj.GetComponent<RectTransform>();

        rect.anchoredPosition = new Vector2(position, 0);

        markers.Add(obj.gameObject);
    }

    public void ClearMarkers()
    {
        foreach (var m in markers)
        {
            Destroy(m);
        }

        markers.Clear();
    }

    public void ClearZones()
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}