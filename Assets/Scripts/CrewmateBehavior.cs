using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CrewmateBehavior : MonoBehaviour
{
    private NavMeshAgent agent;

    private Vector3 topStairsPosition = new Vector3(0, 5, 0);
    private Vector3 bottomStairsPosition = new Vector3(0, 0, 0);

    private int currentDeck = 0;

    private Vector3 finalTarget;
    private int targetDeck;

    private bool navigating = false;
    private bool goingToStairs = false;

    private Action onComplete;

    public GameObject target;
    public AdverseEvent adverseEvent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        DoTask(target.transform.position, 1, adverseEvent);
    }

    void Update()
    {
        if (!navigating) return;

        HandleNavigation();
    }

    public void NavigateTo(Vector3 targetPosition, int deck, Action callback = null)
    {
        finalTarget = targetPosition;
        targetDeck = deck;

        navigating = true;
        goingToStairs = false;
        onComplete = callback;
    }

    private void HandleNavigation()
    {
        // STEP 1: need to change decks
        if (currentDeck != targetDeck)
        {
            Vector3 stairPos =
                (currentDeck == 0) ? topStairsPosition : bottomStairsPosition;

            // only set once
            if (!goingToStairs)
            {
                goingToStairs = true;
                agent.SetDestination(stairPos);
            }

            // arrival check (robust)
            if (HasArrived(stairPos))
            {
                currentDeck = targetDeck;
                agent.Warp((currentDeck == 0) ? topStairsPosition : bottomStairsPosition);

                goingToStairs = false;

                // NOW go to final target
                agent.SetDestination(finalTarget);
            }

            return;
        }

        if (!goingToStairs)
        {
            agent.SetDestination(finalTarget);
            goingToStairs = true; // reuse flag as "path set"
        }

        if (HasArrived(finalTarget))
        {
            navigating = false;
            onComplete?.Invoke();
            goingToStairs = false;
        }
    }

    private bool HasArrived(Vector3 target)
    {
        if (agent.pathPending) return false;

        if (agent.remainingDistance > agent.stoppingDistance + 0.1f)
            return false;

        return true;
    }

    public void DoTask(Vector3 position, int deck, AdverseEvent e)
    {
        NavigateTo(target.transform.position, 1, () =>
        {
            e.BeginFixing();
        });

    }
}