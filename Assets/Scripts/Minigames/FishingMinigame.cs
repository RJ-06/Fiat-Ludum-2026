using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float minWaitTime;
    [SerializeField] private float maxWaitTime;
    [SerializeField] private float timeToCatch;

    [Header("State")]
    private bool canCatch = false;
    private bool hasEnded = false;
    private bool isActive = false;
    private float timer = 0f;

    public float score;

    [Header("UI")]
    [SerializeField] private Image waitingForFishNotification;
    [SerializeField] private Image canCatchNotification;
    [SerializeField] private Image failedCatchNotification;
    [SerializeField] private TMPro.TMP_Text resultText;

    public enum ResourceType { Gold, Wood }

    // ----------------- SCORING -----------------

    float GetThreshold(int level)
    {
        float baseThreshold = 0.5f;
        float reductionPerLevel = 0.1f;
        return Mathf.Clamp01(baseThreshold - level * 0.1f);
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

    // ----------------- PUBLIC ENTRY -----------------

    public IEnumerator StartFishing()
    {
        ExitFishing();

        isActive = true;
        hasEnded = false;

        score = 0;
        timer = 0;
        canCatch = false;

        waitingForFishNotification.enabled = true;
        canCatchNotification.enabled = false;
        failedCatchNotification.enabled = false;

        GameplayModeManager.Instance.SetFishingMode(true);

        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

        if (!isActive) yield break;

        waitingForFishNotification.enabled = false;
        canCatchNotification.enabled = true;

        canCatch = true;

        StartCoroutine(StopFishing());
    }

    // ----------------- AUTO FAIL -----------------

    private IEnumerator StopFishing()
    {
        yield return new WaitForSeconds(timeToCatch);

        if (!isActive || hasEnded) yield break;

        hasEnded = true;
        canCatch = false;
        timer = 0;

        failedCatchNotification.enabled = true;

        yield return new WaitForSeconds(1f);

        if (!isActive) yield break;

        failedCatchNotification.enabled = false;
        canCatchNotification.enabled = false;
        waitingForFishNotification.enabled = false;

        GameplayModeManager.Instance.SetFishingMode(false);
        isActive = false;
    }

    // ----------------- PLAYER INPUT -----------------

    public void tryToCatch()
    {
        if (!isActive || hasEnded) return;

        hasEnded = true;

        if (canCatch)
            score = 1 - timer / timeToCatch;
        else
            score = 0;

        canCatch = false;
        timer = 0;

        StartCoroutine(ShowResultAndExit());
    }

    // ----------------- RESULT -----------------

    private IEnumerator ShowResultAndExit()
    {
        if (!isActive) yield break;

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

        if (!isActive) yield break;

        resultText.text = "";

        waitingForFishNotification.enabled = false;
        canCatchNotification.enabled = false;
        failedCatchNotification.enabled = false;

        GameplayModeManager.Instance.SetFishingMode(false);
        isActive = false;
    }

    // ----------------- CLEAN EXIT (IMPORTANT) -----------------

    public void ExitFishing()
    {
        if (!isActive && !hasEnded)
        {
            ResetUI();
            return;
        }

        isActive = false;
        hasEnded = true;

        StopAllCoroutines();

        canCatch = false;
        timer = 0;

        ResetUI();

        GameplayModeManager.Instance.SetFishingMode(false);
    }

    private void ResetUI()
    {
        waitingForFishNotification.enabled = false;
        canCatchNotification.enabled = false;
        failedCatchNotification.enabled = false;

        if (resultText != null)
            resultText.text = "";
    }

    // ----------------- UPDATE -----------------

    private void FixedUpdate()
    {
        if (canCatch)
            timer += Time.fixedDeltaTime;
    }
}