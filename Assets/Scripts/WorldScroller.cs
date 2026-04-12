using UnityEngine;

public class WorldScroller : MonoBehaviour
{
    [Header("Forward motion")]
    [SerializeField] private float forwardSpeed = 10f;

    [Header("Steering")]
    [SerializeField] private float steerSpeed = 60f;

    void Update()
    {
        // 1. Move world forward toward ship
        transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;

        // 2. Get input ONLY in steering mode
        float input = 0f;

        if (GameplayModeManager.Instance != null &&
            GameplayModeManager.Instance.currentMode == GameplayModeManager.Mode.ShipSteering)
        {
            input = Input.GetAxis("Horizontal");
        }

        // 3. ROTATE WORLD around ship (THIS is the key change)
        transform.RotateAround(
            Vector3.zero,     // ship position (must be at origin or reference transform)
            Vector3.up,
            input * steerSpeed * Time.deltaTime
        );
    }
}