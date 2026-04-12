using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
