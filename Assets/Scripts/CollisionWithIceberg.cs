using UnityEngine;

public class CollisionWithIceberg : MonoBehaviour
{
    [SerializeField] AudioSource icebergHitSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Iceberg"))
        {
            icebergHitSound.Play();
            ShipManager.shipManager.shipHealth -= 10f;
            Destroy(other.gameObject);
        }
    }
}
