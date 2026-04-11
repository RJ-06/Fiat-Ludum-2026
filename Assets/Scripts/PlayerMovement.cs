using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveDir;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxYSpeed;
    private Rigidbody rb;
    [SerializeField] private Transform groundRaycastPos;
    [SerializeField] private float groundRaycastDist;
    private bool atStairs;
    private int currentDeck = 0;
    AdverseEvent currentlyRepairing;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.blue);
    }

    private void FixedUpdate()
    {
        // ROTATION (left/right input)
        float turnSpeed = 120f; // tweak this
        transform.Rotate(Vector3.up, moveDir.x * turnSpeed * Time.deltaTime);

        // FORWARD/BACK movement (relative to player forward)
        Vector3 forwardMove = transform.forward * (moveDir.y * moveSpeed);

        // preserve vertical velocity
        Vector3 desiredMove = new Vector3(
            forwardMove.x,
            rb.linearVelocity.y,
            forwardMove.z
        );

        // GROUND / SLOPE HANDLING
        if (Physics.Raycast(groundRaycastPos.position, Vector3.down, out RaycastHit hit, groundRaycastDist))
        {
            Vector3 slopeMoveDir = Vector3.ProjectOnPlane(desiredMove, hit.normal);
            rb.linearVelocity = slopeMoveDir;
        }
        else
        {
            rb.linearVelocity = new Vector3(desiredMove.x, rb.linearVelocity.y, desiredMove.z);
        }

        // CLAMP Y SPEED
        if (rb.linearVelocity.y > maxYSpeed)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, maxYSpeed, rb.linearVelocity.z);
        }
    }


    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>().normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stairs"))
        {
            atStairs = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stairs"))
        {
            atStairs = false;
        }
    }

    private void OnInteractPress()
    {
        if (atStairs) 
        {
            if (currentDeck == 0) 
            {
                transform.position -= new Vector3(0, 2, 0);
                currentDeck = 1;
            }
            else 
            {
                transform.position += new Vector3(0, 4, 0);
                currentDeck = 0;
            }
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<AdverseEvent>() != null)
            {
                currentlyRepairing = hitCollider.GetComponent<AdverseEvent>();
                currentlyRepairing.BeginFixing();
                break;
            }
        }
    }

    private void OnInteractRelease()
    {
        if (currentlyRepairing != null)
        {
            currentlyRepairing.StopFixing();
            currentlyRepairing = null;
        }
    }


}
