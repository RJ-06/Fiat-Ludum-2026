using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    public enum ObjectType 
    {
        cannonball,
        bucket
    }
    public ObjectType objectType;
    private Rigidbody rb;
    [SerializeField] AudioSource dropSound;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other) {
        if (other.collider.CompareTag("Ship Ground") && rb.linearVelocity.y < 0f)
        {
            dropSound.Play();
        }
    }

}
