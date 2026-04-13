using Unity.VisualScripting;
using UnityEngine;

public class TentacleAttack : MonoBehaviour
{
    [SerializeField] float damage = 20f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            ShipManager.shipManager.shipHealth -= damage;
        }
    }

}
