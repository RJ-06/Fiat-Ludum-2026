using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

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

    [SerializeField] private Animator crewmateAnimator;

    // Cache animation hashes for slightly better performance
    private readonly int isWalkingHash = Animator.StringToHash("isWalking");
    private readonly int isFixingHash = Animator.StringToHash("isFixing");

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // FIX 1: Use the agent's actual velocity to determine if they are walking. 
        // Just checking the 'navigating' bool can cause them to animate while stuck or standing still.
        bool isActuallyMoving = navigating && agent.velocity.sqrMagnitude > 0.01f;
        if (crewmateAnimator != null && agent != null)
        {
            crewmateAnimator.SetBool(isWalkingHash, isActuallyMoving);
        }
        if (isActuallyMoving && !walkSound.isPlaying) walkSound.Play();
        else if (!isActuallyMoving && walkSound.isPlaying) walkSound.Stop();

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
                (currentDeck == 0) ? topStairsPosition.position : bottomStairsPosition.position;

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
                agent.Warp((currentDeck == 0) ? topStairsPosition.position : bottomStairsPosition.position);

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
            // This runs WHEN they arrive
            if (crewmateAnimator != null)
            {
                crewmateAnimator.SetBool(isFixingHash, true);
            }
            e.BeginFixing();

            // FIX 2: Move the Invoke inside the arrival callback!
            StartCoroutine(ReturnToQueueAfterDelay(e.fixTime));
        });

    }

    private IEnumerator ReturnToQueueAfterDelay(float time)
    {
        // Cancel the fixing animation once they are done and back in queue
        if (crewmateAnimator != null)
        {
            crewmateAnimator.SetBool(isFixingHash, false);
        }

        yield return new WaitForSeconds(time);
        ShipManager.shipManager.crewmateQueue.Enqueue(this);
    }
}