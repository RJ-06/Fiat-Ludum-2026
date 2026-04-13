using System.Collections;
using UnityEngine;

public class FireBehavior : MonoBehaviour
{
    [SerializeField] float burnRate;
    [SerializeField] float damageDealt;
    ShipManager shipManagerInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shipManagerInstance = ShipManager.shipManager;
        StartCoroutine(BurnShip());
    }

    IEnumerator BurnShip() 
    {
        while (true) 
        {
            shipManagerInstance.TakeDamage(damageDealt);
            yield return new WaitForSeconds(burnRate);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<GrabbableObject>() != null && 
            other.GetComponent<GrabbableObject>().objectType == GrabbableObject.ObjectType.bucket &&
            !PlayerMovement.instance.isGrabbing &&
            other.GetComponent<BucketScript>().isFilled) 
        { //check if a bucket has been placed which the player is holding and is filled with water
            other.GetComponent<BucketScript>().isFilled = false;
            Destroy(gameObject);
            shipManagerInstance.gold += 30;
        }
    }
}
