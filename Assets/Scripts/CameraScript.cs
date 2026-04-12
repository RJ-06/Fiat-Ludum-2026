using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;

    public Vector3 offset = new Vector3(0, 4, -6);
    public float smoothSpeed = 10f;

    [SerializeField] private Transform wheelPos;
    [SerializeField] private Transform cannonOnePos;
    [SerializeField] private Transform cannonTwoPos;
    [SerializeField] private Transform cookingTable;
    [SerializeField] private Transform fishingSpot;

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
            AlignToForward(wheelPos, Vector3.back);
            return;
        }
        else if (GameplayModeManager.Instance.currentMode == GameplayModeManager.Mode.CannonShooting)
        { //choose between which of the two cannons to use by finding which one you're closer to (the one you're in interaction range of)
            float distanceOne = Vector2.Distance(player.position, cannonOnePos.position);
            float distanceTwo = Vector2.Distance(player.position, cannonTwoPos.position);
            Transform cannonChoice =
                (distanceOne <= distanceTwo) ? cannonChoice = cannonOnePos : cannonTwoPos;

            AlignToForward(cannonChoice, Vector3.right); //DIRECTION HAS TO BE CHANGED ONCE SWITCHED TO LOWER DECK CANNONSa
            return;
        }
        else if (GameplayModeManager.Instance.currentMode == GameplayModeManager.Mode.Cooking)
        {
            AlignToForward(cookingTable, Vector3.down);
            return;
        }
        else if (GameplayModeManager.Instance.currentMode == GameplayModeManager.Mode.Fishing) 
        {
            AlignToForward(fishingSpot, Vector3.right); //SWITCH DIRECTION
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


    void AlignToForward(Transform positionToAlignTo, Vector3 dirToFace)
    {
        // Look straight forward relative to ship/world
        targetRotation = Quaternion.LookRotation(dirToFace, Vector3.up);

        transform.position = Vector3.Lerp(transform.position, positionToAlignTo.position + new Vector3(0,0,1f), smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * smoothSpeed
        );
    }
}