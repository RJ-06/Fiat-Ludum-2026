using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveDir;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxYSpeed;
    private Rigidbody rb;
    [SerializeField] private Transform groundRaycastPos;
    [SerializeField] private float groundRaycastDist;


    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float rotationSpeed = 720f;

    private Vector3 currentVelocity;


    private bool atStairs;
    private int currentDeck = 0; // 0 = top, 1 = bottom

    private bool atMast;
    private int currentMastLevel = 0; // 0 = on deck, 1 = at top
    AdverseEvent currentlyRepairing;

    [SerializeField] Transform topDeckPosition;
    [SerializeField] Transform bottomDeckPosition;
    [SerializeField] Transform topMastPosition;
    [SerializeField] Transform bottomMastPosition;


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
        // ROTATION
        float turnAmount = moveDir.x * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turn = Quaternion.Euler(0f, turnAmount, 0f);
        rb.MoveRotation(rb.rotation * turn);

        // FORWARD (FIXED: use rb.rotation, not transform)
        Vector3 forward = rb.rotation * Vector3.forward;
        Vector3 targetVelocity = forward * (moveDir.y * moveSpeed);

        targetVelocity.y = rb.linearVelocity.y;

        // SLOPE
        if (Physics.Raycast(groundRaycastPos.position, Vector3.down, out RaycastHit hit, groundRaycastDist))
        {
            forward = Vector3.ProjectOnPlane(forward, hit.normal).normalized;
            targetVelocity = forward * (moveDir.y * moveSpeed);
            targetVelocity.y = rb.linearVelocity.y;
        }

        // SMOOTH ONLY XZ
        Vector3 velocity = rb.linearVelocity;

        Vector3 currentXZ = new Vector3(velocity.x, 0f, velocity.z);
        Vector3 targetXZ = new Vector3(targetVelocity.x, 0f, targetVelocity.z);

        Vector3 smoothXZ = Vector3.Lerp(currentXZ, targetXZ, acceleration * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector3(smoothXZ.x, velocity.y, smoothXZ.z);

        // CLAMP Y
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

        if (other.CompareTag("Mast"))
        {
            atMast = true;
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stairs"))
        {
            atStairs = false;
        }

        if (other.CompareTag("Mast"))
        {
            atMast = false;
        }
    }

    private void OnInteractPress()
    {
        if (atStairs) 
        {
            if (currentDeck == 0) 
            {
                StartCoroutine(MoveToTarget(bottomDeckPosition.position));
                currentDeck = 1;
            }
            else 
            {
                StartCoroutine(MoveToTarget(topDeckPosition.position));
                currentDeck = 0;
            }
        }

        if (atMast)
        {
            if (currentMastLevel == 0)
            {
                StartCoroutine(MoveToTarget(new Vector3(transform.position.x, topMastPosition.position.y, transform.position.z)));
                currentMastLevel = 1;
            }
            else
            {
                StartCoroutine(MoveToTarget(new Vector3(transform.position.x, bottomMastPosition.position.y, transform.position.z)));
                currentMastLevel = 0;
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

    public IEnumerator MoveToTarget(Vector3 target, float duration = 1f)
    {
        rb.isKinematic = true;

        Collider col = GetComponent<Collider>();
        float feetOffset = col.bounds.extents.y;

        Vector3 startPos = rb.position;
        Vector3 adjustedTarget = target + Vector3.up * feetOffset;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            rb.MovePosition(Vector3.Lerp(startPos, adjustedTarget, t));

            yield return null;
        }

        rb.MovePosition(adjustedTarget);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = false;
    }
}
