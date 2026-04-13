using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class KrakenScript : MonoBehaviour
{

    public int healthPoints = 100;
    public int damagePerCannonball = 15;

    public UnityEvent OnKrakenDeath;

    [SerializeField] float timeSubmerged;
    [SerializeField] float timeWaiting;
    [SerializeField] float timeLeakAttack;
    [SerializeField] float timeSweepAttack;
    [SerializeField] float variationTime;

    [Header("Submergence Behavior")]
    [SerializeField] float submergeDepth;
    [SerializeField] float regularDepth;

    [Header("Waiting State Movement")]
    [SerializeField] float waitingMoveDistance = 5f;
    [SerializeField] float waitingMoveSpeed = 2f;

    [Header("leak attack")]
    TaskManager taskManager;
    [SerializeField] int numTasksFromAttack = 6;

    [Header("Tentacle Sweep Attack")]
    bool isSweepAttacking;
    [SerializeField] GameObject tentacleOne;
    [SerializeField] float tentacleMoveTime = 1f; // Added a variable to control how fast it rises/falls

    enum bossState
    {
        submerge,
        waiting,
        leakAttack,
        sweepAttack,
    }

    private bossState currentState;

    // Start is called before the first frame update
    void Start()
    {
        taskManager = FindAnyObjectByType<TaskManager>();
        StartCoroutine(StateControl());
    }

    IEnumerator StateControl()
    {
        while (true)
        {
            int choice = Random.Range(0, 4);
            currentState = (bossState)choice;
            float randomVariation = Random.Range(-variationTime, variationTime);
            switch (currentState)
            {
                case bossState.submerge:
                    yield return StartCoroutine(SubmergeKraken(timeSubmerged + randomVariation));
                    break;
                case bossState.waiting:
                    yield return StartCoroutine(KrakenWaiting(timeWaiting + randomVariation));
                    break;
                case bossState.leakAttack:
                    LeakAttack();
                    yield return new WaitForSeconds(timeLeakAttack + randomVariation);
                    break;
                case bossState.sweepAttack:
                    // Changed this to start the sweep coroutine and wait for it to finish
                    yield return StartCoroutine(SweepAttack(timeSweepAttack + randomVariation));
                    break;
                default: break;
            }
            yield return null;
        }
    }

    IEnumerator SubmergeKraken(float duration)
    {
        float elapsed = 0f;
        Vector3 initialPosition = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;

            // Mathf.Sin over progress * PI creates a smooth curve from 0 to 1 and back to 0
            float sinWave = Mathf.Sin(progress * Mathf.PI);
            float currentY = Mathf.Lerp(regularDepth, submergeDepth, sinWave);

            transform.position = new Vector3(initialPosition.x, currentY, initialPosition.z);

            yield return null;
        }

        // Ensure the Kraken ends up exactly back at the regular depth
        transform.position = new Vector3(initialPosition.x, regularDepth, initialPosition.z);
    }

    IEnumerator KrakenWaiting(float duration)
    {
        float elapsed = 0f;
        Vector3 initialPosition = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // Move left and right relative to the initial X position over time
            float xOffset = Mathf.Sin(elapsed * waitingMoveSpeed) * waitingMoveDistance;

            transform.position = new Vector3(initialPosition.x + xOffset, regularDepth, initialPosition.z);

            yield return null;
        }

        // Ensure it ends up exactly at its original X and regular depth
        transform.position = new Vector3(initialPosition.x, regularDepth, initialPosition.z);
    }

    void LeakAttack()
    {
        if (taskManager != null)
        {
            for (int i = 0; i < numTasksFromAttack; i++)
            {
                taskManager.SpawnImmediateTask();
            }
        }
    }

    IEnumerator SweepAttack(float duration)
    {
        isSweepAttacking = true;

        if (tentacleOne != null)
        {
            Vector3 startPos = tentacleOne.transform.position;
            Vector3 hiddenPos = new Vector3(startPos.x, submergeDepth, startPos.z);
            Vector3 raisedPos = new Vector3(startPos.x, regularDepth, startPos.z);

            // 1. Lerp Upwards
            float elapsed = 0f;
            while (elapsed < tentacleMoveTime)
            {
                elapsed += Time.deltaTime;
                tentacleOne.transform.position = Vector3.Lerp(hiddenPos, raisedPos, elapsed / tentacleMoveTime);
                yield return null;
            }
            tentacleOne.transform.position = raisedPos;

            // =========================================================
            // SPACE TO TRIGGER ANIMATION
            // Calculate how much time is left for the attack animation
            float attackDuration = duration - (tentacleMoveTime * 2);
            if (attackDuration > 0)
            {
                // e.g. animator.SetTrigger("SweepAttack");
                yield return new WaitForSeconds(attackDuration);
            }
            // =========================================================

            // 2. Lerp Downwards
            elapsed = 0f;
            while (elapsed < tentacleMoveTime)
            {
                elapsed += Time.deltaTime;
                tentacleOne.transform.position = Vector3.Lerp(raisedPos, hiddenPos, elapsed / tentacleMoveTime);
                yield return null;
            }
            tentacleOne.transform.position = hiddenPos;
        }

        isSweepAttacking = false;
    }

    private void FixedUpdate()
    {
        if (healthPoints <= 0) // Minor adjustment: Better to use <= instead of < for health checks 
        {
            OnKrakenDeath.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CannonBall>() != null)
        {
            healthPoints -= damagePerCannonball;
        }
    }
}