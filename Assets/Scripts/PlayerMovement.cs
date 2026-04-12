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
        // ROTATION (physics-safe)
        Quaternion turn = Quaternion.Euler(0f, moveDir.x * 120f * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * turn);

        // MOVEMENT
        Vector3 forwardMove = transform.forward * (moveDir.y * moveSpeed);

        Vector3 velocity = forwardMove;
        velocity.y = rb.linearVelocity.y;

        // SLOPE HANDLING (smoothed)
        if (Physics.Raycast(groundRaycastPos.position, Vector3.down, out RaycastHit hit, groundRaycastDist))
        {
            Vector3 projected = Vector3.ProjectOnPlane(velocity, hit.normal);
            velocity = Vector3.Lerp(velocity, projected, 15f * Time.fixedDeltaTime);
        }

        // SMOOTH VELOCITY (KEY FIX)
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, velocity, 10f * Time.fixedDeltaTime);

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
                StartCoroutine(MoveToTarget(bottomDeckPosition));
                currentDeck = 1;
            }
            else 
            {
                StartCoroutine(MoveToTarget(topDeckPosition));
                currentDeck = 0;
            }
        }

        if (atMast)
        {
            if (currentMastLevel == 0)
            {
                StartCoroutine(MoveToTarget(topMastPosition));
                currentMastLevel = 1;
            }
            else
            {
                StartCoroutine(MoveToTarget(bottomMastPosition));
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

    public IEnumerator MoveToTarget(Transform target, float duration = 1f)
    {
        rb.isKinematic = true;

        Collider col = GetComponent<Collider>();
        float feetOffset = col.bounds.extents.y;

        Vector3 startPos = rb.position;
        Vector3 adjustedTarget = target.position + Vector3.up * feetOffset;

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
