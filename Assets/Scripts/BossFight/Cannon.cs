using UnityEngine;

public class Cannon : MonoBehaviour
{

    public GameObject cannonBall;
    public Transform ballSpawnPos;
    public float shootForce;

    [Header("Cannon Rotation")]
    public float rotationSpeed = 1.5f; // Rotation speed in radians per second

    [Header("Rotation Limits")]
    public float maxLRAngle = 40f; // Up/Down limits (+/- 30 degrees)
    public float maxUDAngle = 30f;   // Left/Right limits (+/- 40 degrees)

    private float LRAngle = 0f;
    private float UDAngle = 0f;
    private Quaternion startRotation;

    public int cannonBallsLeft;

    private void Start()
    {
        // Store the initial rotation so we know what "0 degrees" looks like
        startRotation = transform.localRotation;
    }

    private void FixedUpdate()
    {
        // 1. Get input ONLY in CannonShooting mode
        if (GameplayModeManager.Instance != null &&
            GameplayModeManager.Instance.currentMode == GameplayModeManager.Mode.CannonShooting)
        {
            // 2. Fetch the moveDir from the player reference in GameplayModeManager
            Vector2 inputDir = PlayerMovement.instance.moveDir;

            // 3. Rotate the cannon based on the input
            // Applying Time.deltaTime ensures the rotation is smooth and frame-rate independent
            RotateCannon(inputDir.x * rotationSpeed * Time.deltaTime, inputDir.y * rotationSpeed * Time.deltaTime);
        }
    }

    public void RotateCannon(float angleLRChange, float angleUDChange)
    {
        float degreesLR = angleLRChange * Mathf.Rad2Deg;
        float degreesUD = angleUDChange * Mathf.Rad2Deg;

        LRAngle += degreesLR;
        UDAngle += degreesUD;
        LRAngle = Mathf.Clamp(LRAngle, -maxUDAngle, maxUDAngle);
        UDAngle = Mathf.Clamp(UDAngle, -maxLRAngle, maxLRAngle);

        transform.localRotation = startRotation * Quaternion.Euler(UDAngle, LRAngle, 0f);
    }

    public void ShootCanonBall()
    {
        if (cannonBallsLeft > 0) 
        {
            GameObject newCB = Instantiate(cannonBall, ballSpawnPos.position, Quaternion.identity);
            Rigidbody rb = newCB.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * shootForce, ForceMode.Impulse);
            cannonBallsLeft--;
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!PlayerMovement.instance.isGrabbing && other.GetComponent<GrabbableObject>() != null
            && other.GetComponent<GrabbableObject>().objectType == GrabbableObject.ObjectType.cannonball) 
        {
            cannonBallsLeft++;
            other.gameObject.GetComponent<Collider>().enabled = false;
            other.GetComponent<GrabbableObject>().enabled = false;
            Destroy(other.gameObject);
        }
    }
}