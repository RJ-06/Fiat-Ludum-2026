using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    public Vector2 moveDir;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxYSpeed;
    private Rigidbody rb;
    [SerializeField] private Transform groundRaycastPos;
    [SerializeField] private float groundRaycastDist;


    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float rotationSpeed = 720f;


    private bool atStairs;
    private int currentDeck = 0; // 0 = top, 1 = bottom

    private bool atMast;
    private int currentMastLevel = 0; // 0 = on deck, 1 = at top

    private bool atWheel;

    private bool atKitchen;

    private bool atCannon;
    private Cannon currentCannon;

    private bool atFishing;

    public bool isGrabbing;
    [SerializeField] Transform grabPosition;
    private Transform objectGrabbedTransform;
    private GameObject objectFoundToGrab;

    AdverseEvent currentlyRepairing;

    [SerializeField] Transform topDeckPosition;
    [SerializeField] Transform bottomDeckPosition;
    [SerializeField] Transform topMastPosition;
    [SerializeField] Transform bottomMastPosition;

    [SerializeField] private Animator playerAnimator;
    [SerializeField] AudioSource walkSound;

    public Renderer playerRend;

    // Animation Hashes
    private readonly int isWalkingHash = Animator.StringToHash("isWalking");
    private readonly int isClimbingHash = Animator.StringToHash("isClimbing");
    private readonly int isFixingHash = Animator.StringToHash("isFixing");
    private readonly int dropObjectHash = Animator.StringToHash("dropObject");
    private readonly int climbSpeedHash = Animator.StringToHash("climbSpeed"); // Multiplier for forward/backward climbing

    private void Awake()
    {
        instance = this;
        playerRend = GetComponentInChildren<Renderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Player is considered walking if there's movement input, they aren't fixing, and not climbing (isKinematic is true during climbing)
        bool isWalking = moveDir.sqrMagnitude > 0.01f && currentlyRepairing == null && !rb.isKinematic;

        if (playerAnimator != null)
        {
            playerAnimator.SetBool(isWalkingHash, isWalking);
        }

        if (!walkSound.isPlaying && isWalking) walkSound.Play();
        else if (walkSound.isPlaying && !isWalking) walkSound.Stop();
    }

    private void FixedUpdate()
    {
        if (!GameplayModeManager.Instance.isWalkingMode())
            return; // ignore input in steering mode

        if (currentlyRepairing != null)
            return; // ignore movement input while repairing

        if (isGrabbing)
        {
            objectGrabbedTransform.position = grabPosition.position;
        }

        // ROTATION
        float turnAmount = moveDir.x * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turn = Quaternion.Euler(0f, turnAmount, 0f);
        rb.MoveRotation(rb.rotation * turn);

        // FORWARD (FIXED: use rb.rotation, not transform)
        Vector3 forward = rb.rotation * Vector3.forward;
        Vector3 targetVelocity = forward * (moveDir.y * moveSpeed * ShipManager.shipManager.actingSpeedMultiplier * ShipManager.shipManager.speedMultiplier);

        targetVelocity.y = rb.linearVelocity.y;

        // SLOPE
        if (Physics.Raycast(groundRaycastPos.position, Vector3.down, out RaycastHit hit, groundRaycastDist))
        {
            forward = Vector3.ProjectOnPlane(forward, hit.normal).normalized;
            targetVelocity = forward * (moveDir.y * moveSpeed * ShipManager.shipManager.actingSpeedMultiplier * ShipManager.shipManager.speedMultiplier);
            targetVelocity.y = rb.linearVelocity.y;
        }

        // SMOOTH ONLY XZ
        Vector3 velocity = rb.linearVelocity;

        Vector3 currentXZ = new Vector3(velocity.x, 0f, velocity.z);
        Vector3 targetXZ = new Vector3(targetVelocity.x, 0f, targetVelocity.z);

        Vector3 smoothXZ = Vector3.Lerp(currentXZ, targetXZ, acceleration * Time.fixedDeltaTime);

        if(!rb.isKinematic) rb.linearVelocity = new Vector3(smoothXZ.x, velocity.y, smoothXZ.z);

        // CLAMP Y
        if (rb.linearVelocity.y > maxYSpeed && !rb.isKinematic)
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
        if (other.CompareTag("Wheel"))
        {
            atWheel = true;
        }
        if (other.CompareTag("Cannon"))
        {
            atCannon = true;
            currentCannon = other.gameObject.GetComponent<Cannon>();
        }
        if (other.CompareTag("Grabbable"))
        {
            objectFoundToGrab = other.gameObject;
        }
        if (other.CompareTag("Kitchen"))
        {
            atKitchen = true;
        }
        if (other.CompareTag("Fishing"))
        {
            atFishing = true;
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
        if (other.CompareTag("Wheel"))
        {
            atWheel = false;
        }
        if (other.CompareTag("Cannon"))
        {
            atCannon = false;
            currentCannon = null;
        }
        if (other.CompareTag("Grabbable"))
        {
            objectFoundToGrab = null;
        }
        if (other.CompareTag("Kitchen"))
        {
            atKitchen = false;
        }
        if (other.CompareTag("Fishing"))
        {
            atFishing = false;
        }
    }

    private void OnInteractPress()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<AdverseEvent>() != null)
            {
                currentlyRepairing = hitCollider.GetComponent<AdverseEvent>();
                currentlyRepairing.BeginFixing();
                rb.linearVelocity = Vector3.zero; // stop player movement immediately when starting to repair
                if (playerAnimator != null) playerAnimator.SetBool(isFixingHash, true); // Trigger fixing animation
                return;
            }
        }
        if (!isGrabbing && objectFoundToGrab != null)
        {
            isGrabbing = true;
            objectGrabbedTransform = objectFoundToGrab.transform;
            objectGrabbedTransform.position = grabPosition.position;
            objectFoundToGrab.GetComponent<Rigidbody>().useGravity = false;
            objectFoundToGrab.GetComponent<Rigidbody>().isKinematic = true;
            objectGrabbedTransform.SetParent(transform); //parents the object you're holding to the player
        }


        if (atStairs)
        {
            if (currentDeck == 0)
            {
                StartCoroutine(MoveToTarget(bottomDeckPosition.position));
                ChangeShipVisibility.Instance.ToggleVisibilityTop();
                currentDeck = 1;
            }
            else
            {
                StartCoroutine(MoveToTarget(topDeckPosition.position));
                ChangeShipVisibility.Instance.ToggleVisibilityTop();
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

        if (atWheel)
        {
            GameplayModeManager.Instance.SetShipSteeringMode(true);
        }

        if (atKitchen)
        {
            GameplayModeManager.Instance.SetCookingMode(true);
            FindAnyObjectByType<SliceMinigameController>().StartMinigame();
        }

        if (atFishing)
        {
            FishingMinigame fishing = FindAnyObjectByType<FishingMinigame>();
            if (GameplayModeManager.Instance.currentMode == GameplayModeManager.Mode.Fishing)
            {
                fishing.tryToCatch();
            }
            else
            {
                GameplayModeManager.Instance.SetFishingMode(true);
                StartCoroutine(fishing.StartFishing());
            }

        }

        if (atCannon)
        {
            if (GameplayModeManager.Instance.currentMode == GameplayModeManager.Mode.CannonShooting)
            {
                currentCannon.ShootCanonBall();
            }
            else 
            {
                GameplayModeManager.Instance.SetCannonShootingMode(true);
                ChangeShipVisibility.Instance.ToggleVisibilityBottom();
            }
        }
    }

    private void OnInteractRelease()
    {
        if (currentlyRepairing != null)
        {
            currentlyRepairing.StopFixing();
            currentlyRepairing = null;
            if (playerAnimator != null) playerAnimator.SetBool(isFixingHash, false); // Stop fixing animation
        }
    }

    private void OnCommand()
    {
        if (isGrabbing)
        {
            isGrabbing = false;
            objectGrabbedTransform.gameObject.GetComponent<Rigidbody>().useGravity = true;
            objectGrabbedTransform.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            objectGrabbedTransform.SetParent(null); //unparents
            objectGrabbedTransform = null;
            if (playerAnimator != null) playerAnimator.SetTrigger(dropObjectHash); // Trigger drop/throw animation
            return;
        }
        if (atWheel)
        {
            GameplayModeManager.Instance.SetShipSteeringMode(false);
        }
        if (atCannon)
        {
            GameplayModeManager.Instance.SetCannonShootingMode(false);
            ChangeShipVisibility.Instance.ToggleVisibilityBottom();
        }
        if (atKitchen)
        {
            GameplayModeManager.Instance.SetCookingMode(false);
            FindAnyObjectByType<SliceMinigameController>().ExitMinigame();
        }
        if (atFishing)
        {
            GameplayModeManager.Instance.SetFishingMode(false);
            FindAnyObjectByType<FishingMinigame>().ExitFishing();
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<AdverseEvent>() != null)
            {
                if (ShipManager.shipManager.crewmateQueue.Count != 0)
                {
                    CrewmateBehavior crewmate = ShipManager.shipManager.crewmateQueue.Dequeue();
                    crewmate.DoTask(hitCollider.transform.position, currentDeck, hitCollider.GetComponent<AdverseEvent>());
                    break;
                }
            }
        }
    }

    public IEnumerator MoveToTarget(Vector3 target, float duration = 1f)
    {
        rb.isKinematic = true;

        Collider col = GetComponent<Collider>();
        float feetOffset = col.bounds.extents.y;

        Vector3 startPos = rb.position;
        Vector3 adjustedTarget = target + Vector3.up * feetOffset;

        // Check if the target is physically higher than where we are starting
        float climbDirection = (adjustedTarget.y > startPos.y) ? 1f : -1f;

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat(climbSpeedHash, climbDirection); // Set the speed multiplier
            playerAnimator.SetBool(isClimbingHash, true); // Start climbing animation
        }

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

        if (playerAnimator != null) playerAnimator.SetBool(isClimbingHash, false); // Stop climbing animation
    }
}