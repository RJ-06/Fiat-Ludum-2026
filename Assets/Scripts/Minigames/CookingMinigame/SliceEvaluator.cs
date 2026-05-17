using System.Collections.Generic;
using UnityEngine;

public class SliceEvaluator : MonoBehaviour
{
    [Header("Bounds")]
    public float leftBound = -300f;
    public float rightBound = 300f;

    public List<SliceZone> zones = new List<SliceZone>();

    public void GenerateZones()
    {
        zones.Clear();

        int zoneCount = Random.Range(1, 4); // 1–3 slices

        float minSeparation = 25f;
        int maxAttempts = 50;

        for (int i = 0; i < zoneCount; i++)
        {
            bool placed = false;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                float pos = Random.Range(leftBound, rightBound);

                float radius = Random.Range(8f, 15f); // thin slices

                SliceZone newZone = new SliceZone(pos, radius);

                if (IsValidZone(newZone, minSeparation))
                {
                    zones.Add(newZone);
                    placed = true;
                    break;
                }
            }

            // fallback if we failed to place cleanly
            if (!placed)
            {
                float pos = Random.Range(leftBound, rightBound);
                zones.Add(new SliceZone(pos, 8f));
            }
        }

        Debug.Log($"Generated {zones.Count} non-overlapping slices");
    }

    private bool IsValidZone(SliceZone candidate, float minSeparation)
    {
        foreach (var zone in zones)
        {
            float minAllowedDistance = zone.radius + candidate.radius + minSeparation;

            float actualDistance = Mathf.Abs(zone.position - candidate.position);

            if (actualDistance < minAllowedDistance)
            {
                return false; // too close or overlapping
            }
        }

        return true;
    }

    // Returns best score for THIS slice only
    public float Evaluate(float value)
    {
        float bestScore = 0f;

        foreach (var zone in zones)
        {
            float distanceToCenter = Mathf.Abs(value - zone.position);

            if (distanceToCenter <= zone.radius)
            {
                return 1f;
            }

            // Distance from the EDGE of the slice (not center)
            float distanceFromEdge = distanceToCenter - zone.radius;

            float maxFalloff = zone.radius; // tweak for how forgiving it is

            float normalized = distanceFromEdge / maxFalloff;

            float score = Mathf.Clamp01(1f - normalized);

            bestScore = Mathf.Max(bestScore, score);
        }

        return bestScore;
    }
}