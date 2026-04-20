using System.Collections.Generic;
using UnityEngine;

public class ResetPositionOfFallen : MonoBehaviour
{
    [SerializeField] List<string> validTags;
    [SerializeField] Transform resetPos;
    private HashSet<string> validTagsSet;

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
        if (validTagsSet.Contains(other.tag)) 
        {
            other.transform.position = resetPos.position;
        }
    }
}
