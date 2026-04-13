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

    private bool hasEnded = false;

    // ----------------- SCORING -----------------

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

        if (score < threshold)
            return (ResourceType.Gold, 0);

        ResourceType type = (ResourceType)Random.Range(0, 2);

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

    // ----------------- UPDATE -----------------

    public void FixedUpdate()
    {
        if (canCatch)
        {
            timer += Time.fixedDeltaTime;
        }

        if (!GameplayModeManager.Instance.isFishingMode())
        {
            waitingForFishNotification.enabled = false;
            canCatchNotification.enabled = false;
            failedCatchNotification.enabled = false;
        }
    }

    // ----------------- START FISHING -----------------

    public IEnumerator StartFishing()
    {
        score = 0;
        timer = 0;
        hasEnded = false;
        canCatch = false;

        waitingForFishNotification.enabled = true;
        canCatchNotification.enabled = false;
        failedCatchNotification.enabled = false;

        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

        waitingForFishNotification.enabled = false;
        canCatchNotification.enabled = true;

        canCatch = true;

        StartCoroutine(StopFishing());
    }

    // ----------------- AUTO FAIL TIMER -----------------

    public IEnumerator StopFishing()
    {
        yield return new WaitForSeconds(timeToCatch);

        if (hasEnded) yield break;

        hasEnded = true;

        canCatch = false;
        timer = 0;

        failedCatchNotification.enabled = true;

        yield return new WaitForSeconds(1f);

        failedCatchNotification.enabled = false;
        canCatchNotification.enabled = false;
        waitingForFishNotification.enabled = false;

        GameplayModeManager.Instance.SetFishingMode(false);
    }

    // ----------------- PLAYER INPUT -----------------

    public void tryToCatch()
    {
        if (hasEnded) return;

        hasEnded = true;

        if (canCatch)
        {
            score = 1 - timer / timeToCatch;
        }
        else
        {
            score = 0;
        }

        canCatch = false;
        timer = 0;

        StartCoroutine(ShowResultAndExit());
    }

    // ----------------- RESULT FLOW -----------------

    IEnumerator ShowResultAndExit()
    {
        var reward = GetReward(score, ShipManager.shipManager.fishingLevel);

        switch (reward.type)
        {
            case ResourceType.Gold:
                ShipManager.shipManager.gold += reward.amount;
                break;

            case ResourceType.Wood:
                ShipManager.shipManager.shipHealth += reward.amount;
                ShipManager.shipManager.shipHealth =
                    Mathf.Clamp(ShipManager.shipManager.shipHealth, 0, 100);
                break;
        }
        canCatchNotification.enabled = false;

        resultText.text =
            $"Your score was {score:F2}. You caught {reward.type} x{reward.amount}!";

        yield return new WaitForSeconds(2f);

        resultText.text = "";

        waitingForFishNotification.enabled = false;
        canCatchNotification.enabled = false;
        failedCatchNotification.enabled = false;

        GameplayModeManager.Instance.SetFishingMode(false);
    }
}