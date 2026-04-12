using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TownspersonEnemy : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private GameObject player;
    [SerializeField] List<Vector3> patrolPoints = new List<Vector3>();
    private int currentPatrolPoint;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.Find("Player");
        currentPatrolPoint = 0;

        StartCoroutine("Patrol");
    }

    void FixedUpdate()
    {
    }

    IEnumerator Patrol()
    {
        yield return new WaitForSeconds(2);
        while (true)
        {
            if (currentPatrolPoint < patrolPoints.Count - 1)
                ++currentPatrolPoint;
            else
                currentPatrolPoint = 0;
            agent.SetDestination(patrolPoints[currentPatrolPoint]);
            yield return new WaitUntil(() => Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint]) < 0.5f);
            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator Chase()
    {
        agent.SetDestination(player.transform.position);
        yield return new WaitForSeconds(3);
    }
}
