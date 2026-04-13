using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class KrakenScript : MonoBehaviour
{
    public int healthPoints = 100;
    public int damagePerCannonball = 15;
    [SerializeField] Animator krakenAnimator;

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
    [SerializeField] float tentacleMoveTime = 1f;
    [SerializeField] Animator tentacleAnimator;
    [SerializeField] float tentacleRaiseYPos;
    [SerializeField] float tentacleSubmergedYPos;

    public enum bossState
    {
        submerge,
        waiting,
        leakAttack,
        sweepAttack,
    }

    public bossState currentState;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        taskManager = FindAnyObjectByType<TaskManager>();
        OnKrakenDeath.AddListener(OnDeathAnim);

        StartCoroutine(StateControl());
    }

    IEnumerator StateControl()
    {
        while (!isDead)
        {
            int choice = Random.Range(0, 4);
            currentState = (bossState)choice;

            // Generate a variation and clamp the total duration to be > 0
            float randomVariation = Random.Range(-variationTime, variationTime);

            switch (currentState)
            {
                case bossState.submerge:
                    yield return StartCoroutine(SubmergeKraken(Mathf.Max(0, timeSubmerged + randomVariation)));
                    break;
                case bossState.waiting:
                    yield return StartCoroutine(KrakenWaiting(Mathf.Max(0, timeWaiting + randomVariation)));
                    break;
                case bossState.leakAttack:
                    LeakAttack();
                    yield return new WaitForSeconds(Mathf.Max(0, timeLeakAttack + randomVariation));
                    break;
                case bossState.sweepAttack:
                    yield return StartCoroutine(SweepAttack(Mathf.Max(0, timeSweepAttack + randomVariation)));
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

            float sinWave = Mathf.Sin(progress * Mathf.PI);
            float currentY = Mathf.Lerp(regularDepth, submergeDepth, sinWave);

            transform.position = new Vector3(initialPosition.x, currentY, initialPosition.z);

            yield return null;
        }

        transform.position = new Vector3(initialPosition.x, regularDepth, initialPosition.z);
    }

    IEnumerator KrakenWaiting(float duration)
    {
        float elapsed = 0f;
        Vector3 initialPosition = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float xOffset = Mathf.Sin(elapsed * waitingMoveSpeed) * waitingMoveDistance;

            transform.position = new Vector3(initialPosition.x + xOffset, regularDepth, initialPosition.z);

            yield return null;
        }

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
            Vector3 hiddenPos = new Vector3(startPos.x, tentacleSubmergedYPos, startPos.z);
            Vector3 raisedPos = new Vector3(startPos.x, tentacleRaiseYPos, startPos.z);

            tentacleOne.transform.position = hiddenPos;

            // 1. Lerp Upwards
            float elapsed = 0f;
            while (elapsed < tentacleMoveTime)
            {
                elapsed += Time.deltaTime;
                tentacleOne.transform.position = Vector3.Lerp(hiddenPos, raisedPos, elapsed / tentacleMoveTime);
                yield return null;
            }
            tentacleOne.transform.position = raisedPos;

            // Trigger animations
            float attackDuration = duration - (tentacleMoveTime * 2);
            if (attackDuration > 0)
            {
                if (tentacleAnimator != null)
                {
                    tentacleAnimator.SetTrigger("SweepAttack");
                }

                yield return new WaitForSeconds(attackDuration);
            }

            // 2. Lerp Downwards
            elapsed = 0f;
            while (elapsed < tentacleMoveTime)
            {
                elapsed += Time.deltaTime;
                tentacleOne.transform.position = Vector3.Lerp(raisedPos, hiddenPos, elapsed / tentacleMoveTime);
                yield return null;
            }
            tentacleOne.transform.position = hiddenPos;

            if (tentacleAnimator != null) tentacleAnimator.ResetTrigger("SweepAttack");
        }

        isSweepAttacking = false;
    }

    public void OnDeathAnim()
    {
        krakenAnimator.SetTrigger("Die");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        // TryGetComponent applies zero garbage footprint if missing compared to GetComponent
        if (other.gameObject.TryGetComponent<CannonBall>(out _))
        {
            healthPoints -= damagePerCannonball;

            if (healthPoints <= 0)
            {
                Die();
            }
            else
            {
                krakenAnimator.SetTrigger("TakeDamage");
            }
        }
    }

    private void Die()
    {
        isDead = true;

        // Stop all active Coroutine attacks and behavior so the boss doesn't keep fighting
        StopAllCoroutines();

        OnKrakenDeath.Invoke();
    }
}