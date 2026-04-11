using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;

    public Vector3 offset = new Vector3(0, 5, -6);
    public float smoothSpeed = 10f;

    void AssignPlayer()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
            player = p.transform;
    }
    void LateUpdate()
    {
        if (player == null)
        {
            AssignPlayer();
            if (player == null) return;
        }

        // Rotate offset based on player rotation
        Vector3 rotatedOffset = player.rotation * offset;

        // Target position
        Vector3 targetPosition = player.position + rotatedOffset;

        // Smooth movement
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Always look at player
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }
}