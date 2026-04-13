using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CrewmateBehavior : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] Transform topStairsPosition;
    [SerializeField] Transform bottomStairsPosition;

    private int currentDeck = 0;

    private Vector3 finalTarget;
    private int targetDeck;

    private bool navigating = false;
    private bool goingToStairs = false;

    private Action onComplete;

    private Animator crewmateAnimator;

    // Optional: Cache animation hashes for slightly better performance
    private readonly int isWalkingHash = Animator.StringToHash("isWalking");
    private readonly int isFixingHash = Animator.StringToHash("isFixing");

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        crewmateAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // Update walking animation based on whether the crewmate is navigating
        if (crewmateAnimator != null)
        {
            crewmateAnimator.SetBool(isWalkingHash, navigating);
        }

        if (!navigating) return;

        HandleNavigation();
    }

    public void NavigateTo(Vector3 targetPosition, int deck, Action callback = null)
    {
        // Cancel fixing animation if they are interrupted / start moving
        if (crewmateAnimator != null)
        {
            crewmateAnimator.SetBool(isFixingHash, false);
        }

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
                (currentDeck == 0) ? topStairsPosition.transform.position : bottomStairsPosition.transform.position;

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
                agent.Warp((currentDeck == 0) ? topStairsPosition.transform.position : bottomStairsPosition.transform.position);

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
        NavigateTo(position, deck, () =>
        {
            if (crewmateAnimator != null)
            {
                crewmateAnimator.SetBool(isFixingHash, true);
            }
            e.BeginFixing();
        });

    }
}