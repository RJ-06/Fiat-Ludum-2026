using UnityEngine;

public class CollisionWithIceberg : MonoBehaviour
{
    [SerializeField] AudioSource icebergHitSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Iceberg"))
        {
            icebergHitSound.Play();
            Debug.Log("Ship hit iceberg!");
        }
    }
}
