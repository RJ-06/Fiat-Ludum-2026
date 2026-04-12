using UnityEngine;
using System.Collections;

public class TownspersonEnemy : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private GameObject player;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        StartCoroutine("UpdatePathfinding");
    }

    IEnumerator UpdatePathfinding()
    {
        agent.SetDestination(player.transform.position);
        yield return new WaitForSeconds(3);
    }
}
