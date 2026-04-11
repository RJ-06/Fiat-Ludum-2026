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
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //rb.linearVelocity = new Vector3(moveDir.x * moveSpeed, rb.linearVelocity.y, moveDir.y * moveSpeed);
        //if (Physics.Raycast(groundRaycastPos.position, Vector3.down, out RaycastHit hit, 5f))
        //{
        //    Vector3 targetPosition = rb.position;
        //    targetPosition.y = hit.point.y;
        //    rb.MovePosition(targetPosition);
        //}

        Vector3 desiredMove = new Vector3(moveDir.x * moveSpeed, rb.linearVelocity.y, moveDir.y * moveSpeed);

        if (Physics.Raycast(groundRaycastPos.position, Vector3.down, out RaycastHit hit, groundRaycastDist))
        {//slope - clamp you to the ground
            Vector3 slopeMoveDir = Vector3.ProjectOnPlane(desiredMove, hit.normal);
            rb.linearVelocity = slopeMoveDir;
        }
        else
        {//regular
            rb.linearVelocity = new Vector3(desiredMove.x, rb.linearVelocity.y, desiredMove.z);
        }

        if (rb.linearVelocity.y > maxYSpeed) 
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, maxYSpeed, rb.linearVelocity.z);
        }
        //Debug.Log(rb.linearVelocity);

    }


    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>().normalized;
    }
}
