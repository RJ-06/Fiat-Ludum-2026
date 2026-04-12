using UnityEngine;

public class CollisionWithIceberg : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Iceberg"))
            Debug.Log("Ship hit iceberg!");
    }
}
