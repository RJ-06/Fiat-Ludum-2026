using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    [SerializeField] private float minWaitTime;
    [SerializeField] private float maxWaitTime;
    [SerializeField] private float timeToCatch;

    [SerializeField] private bool canCatch = false;
    [SerializeField] private float timer = 0f;
    public float score;

    [SerializeField] Image waitingForFishNotification;
    [SerializeField] Image canCatchNotification;
    [SerializeField] Image failedCatchNotification;

    public TMPro.TMP_Text resultText;
    public enum ResourceType { Gold, Wood }

    bool hasEnded = false;  
    float GetThreshold(int level)
    {
        float baseThreshold = 0.5f;
        float reductionPerLevel = 0.1f;
        return Mathf.Clamp01(baseThreshold - level * reductionPerLevel);
    }

    int GetScaledMax(int min, int max, int level)
    {
        float t = level / 4f;
        return Mathf.RoundToInt(Mathf.Lerp(min, max, t));
    }
    public (ResourceType type, int amount) GetReward(float score, int level)
    {
        float threshold = GetThreshold(level);

        // Fail = no reward
        if (score < threshold)
            return (ResourceType.Gold, 0);

        // Pick random resource
        ResourceType type = (ResourceType)Random.Range(0, 3);

        int maxAmount = 0;

        switch (type)
        {
            case ResourceType.Gold:
                maxAmount = GetScaledMax(10, 50, level);
                break;

            case ResourceType.Wood:
                maxAmount = GetScaledMax(4, 20, level);
                break;
        }

        float scoreFactor = Mathf.InverseLerp(threshold, 1f, score);

        int finalAmount = Mathf.RoundToInt(maxAmount * scoreFactor);

        return (type, finalAmount);
    }
    public void FixedUpdate()
    {
        if (canCatch) 
        {
            timer += Time.fixedDeltaTime;
        }
        if (!GameplayModeManager.Instance.isFishingMode()) 
        {
            canCatchNotification.enabled = false;
            failedCatchNotification.enabled = false;
            waitingForFishNotification.enabled = false;
            if (!GameplayModeManager.Instance.isFishingMode())
            {
                canCatchNotification.enabled = false;
                failedCatchNotification.enabled = false;
                waitingForFishNotification.enabled = false;
            }
        }
    }

    public IEnumerator StartFishing()
    {
        score = 0;
        timer = 0;
        hasEnded = false;
        canCatchNotification.enabled = false;
        failedCatchNotification.enabled = false;
        waitingForFishNotification.enabled = true;
        canCatch = false;
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        canCatchNotification.enabled = true;
        canCatch = true;
        StartCoroutine(StopFishing());

    }

    public IEnumerator StopFishing()
    {
       
        yield return new WaitForSeconds(timeToCatch);
        if (canCatch) 
        {
            yield return null;
        }

        if (hasEnded) yield break;

        hasEnded = true;
        canCatchNotification.enabled = false;
        canCatch = false;
        timer = 0;
        if(score == 0)failedCatchNotification.enabled = true;
        yield return new WaitForSeconds(1f);
        canCatchNotification.enabled = false;
        failedCatchNotification.enabled = false;
        waitingForFishNotification.enabled = false;
    }

    public void tryToCatch()
    {
        if (hasEnded) return;

        hasEnded = true;
        if (canCatch)
        {
            canCatch = false;
            canCatchNotification.enabled = false;
            score = 1 - timer / timeToCatch;
            timer = 0;
            StartCoroutine(ShowResultAndExit());

            canCatchNotification.enabled = false;
            failedCatchNotification.enabled = false;
            waitingForFishNotification.enabled = false;
        }
        if (!canCatch) 
        {
            canCatchNotification.enabled = false;
            failedCatchNotification.enabled = false;
            waitingForFishNotification.enabled = false;
            StartCoroutine(ShowResultAndExit());
            timer = 0;
            score = 0;
        }
    }
    IEnumerator ShowResultAndExit()
    {
        (ResourceType type, int amount) reward = GetReward(score, ShipManager.shipManager.fishingLevel);
        switch (reward.type)
        {
            case ResourceType.Gold:
                ShipManager.shipManager.gold += reward.amount;
                break;
            case ResourceType.Wood:
                ShipManager.shipManager.shipHealth += reward.amount;
                ShipManager.shipManager.shipHealth = Mathf.Clamp(ShipManager.shipManager.shipHealth, 0, 100);
                break;
        }

        resultText.text = $"Your score was {score}. You caught {(reward.type == ResourceType.Gold ? "Gold" : "Wood")} x{reward.amount}!";
        // Show result text for 2 seconds
        yield return new WaitForSeconds(2f);

        resultText.text = "";

        canCatchNotification.enabled = false;
        failedCatchNotification.enabled = false;
        waitingForFishNotification.enabled = false;

        GameplayModeManager.Instance.SetFishingMode(false);
    }
}
