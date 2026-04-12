using UnityEngine;

public class WorldScroller : MonoBehaviour
{
    [Header("Forward motion")]
    [SerializeField] private float forwardSpeed = 10f;

    [Header("Steering")]
    [SerializeField] private float steerSpeed = 60f;

    public GameObject iceberg;

    void Update()
    {
        // 1. Move world forward toward ship
        transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;

        // 2. Get input ONLY in steering mode
        float input = 0f;

        if (GameplayModeManager.Instance != null &&
            GameplayModeManager.Instance.currentMode == GameplayModeManager.Mode.ShipSteering)
        {
            input = -Input.GetAxis("Horizontal");
        }

        // 3. ROTATE WORLD around ship (THIS is the key change)
        transform.RotateAround(
            Vector3.zero,     // ship position (must be at origin or reference transform)
            Vector3.up,
            input * steerSpeed * Time.deltaTime
        );
    }

    public void SpawnIceberg()
    {
        int numToInstantiate = Random.Range(1, 4);

        for (int i = 0; i < numToInstantiate; i++)
        {
            float x = -10 + 20 * Random.value;
            float y = 4 * Random.value;
            float z = -60 - 50 * Random.value;

            Instantiate(iceberg, new Vector3(x, y, z), Quaternion.identity, this.transform);
        }
    }
}