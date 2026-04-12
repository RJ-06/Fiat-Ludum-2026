using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;

public class TownspersonEnemy : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private GameObject player;
    [SerializeField] List<Vector3> patrolPoints = new List<Vector3>();
    [SerializeField] float visionRange = 50f, visionAngle = 90f;
    private int currentPatrolPoint;
    private bool patrolling = false, chasing = false;
    private LayerMask layerMask;
    private MeshRenderer meshRenderer;
    [SerializeField] Material patrolMaterial, chaseMaterial;

    void Start()
    {
        layerMask = LayerMask.GetMask("Player", "Default");
        meshRenderer = GetComponent<MeshRenderer>();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.Find("Player");
        currentPatrolPoint = 0;
    }

    void FixedUpdate()
    {
        if (CheckForPlayer()) {
            patrolling = false;
            chasing = true;
            StopCoroutine("Patrol");
            StartCoroutine("Chase");
            agent.speed = 6;
            meshRenderer.material = chaseMaterial;
        }
        else if (!patrolling && !CheckForPlayer()) {
            chasing = false;
            patrolling = true;
            StopCoroutine("Chase");
            StartCoroutine("Patrol");
            agent.speed = 4;
            meshRenderer.material = patrolMaterial;
        }
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
            yield return new WaitUntil(() => Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint]) < 1.5f);
            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator Chase()
    {
        agent.SetDestination(player.transform.position);
        yield return new WaitForSeconds(3);
    }

    bool CheckForPlayer()
    {
        if (player == null) return false;

        if (Vector3.Distance(transform.position, player.transform.position) > visionRange) return false;

        Vector3 toPlayer = player.transform.position - transform.position;
        Vector3 visionDirection = transform.forward;

        if (Vector2.Angle(visionDirection, toPlayer) > visionAngle / 2) return false;

        RaycastHit hit;
        if (!Physics.Raycast(transform.position, toPlayer.normalized, out hit, visionRange, layerMask))
           return false;

        if (hit.collider.gameObject != player)
           return false;

        return true;
    }
}
