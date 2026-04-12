using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;

    public Vector3 offset = new Vector3(0, 4, -6);
    public float smoothSpeed = 10f;

    [SerializeField] private Transform ship;

    private Quaternion targetRotation;

    void AssignPlayer()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    void LateUpdate()
    {
        if (GameplayModeManager.Instance.currentMode ==
            GameplayModeManager.Mode.ShipSteering)
        {
            AlignToForward();
            return;
        }

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


    void AlignToForward()
    {
        // Look straight forward relative to ship/world
        targetRotation = Quaternion.LookRotation(-Vector3.forward, Vector3.up);

        transform.position = Vector3.Lerp(transform.position, ship.position + new Vector3(0,0,1f), smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * smoothSpeed
        );
    }
}