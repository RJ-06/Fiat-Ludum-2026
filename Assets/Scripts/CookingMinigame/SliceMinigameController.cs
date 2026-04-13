using UnityEngine;
using TMPro;
using System.Collections;

public class SliceMinigameController : MonoBehaviour
{
    [SerializeField] private SliderMover sliderMover;
    [SerializeField] private SliceEvaluator evaluator;
    [SerializeField] private SliceZoneVisualizer visualizer;

    [Header("UI")]
    [SerializeField] private GameObject minigameUIRoot;   // whole UI panel
    [SerializeField] private TextMeshProUGUI resultText;

    private bool isPlaying;

    private int attemptsUsed;
    private int maxAttempts;

    private float totalScore;

    public void StartMinigame()
    {
        isPlaying = true;
        minigameUIRoot.SetActive(true);

        evaluator.GenerateZones();
        visualizer.DrawZones(evaluator.zones);

        attemptsUsed = 0;
        maxAttempts = evaluator.zones.Count;
        totalScore = 0f;

        sliderMover.StartMoving();
    }

    public void OnSlicePressed(float value)
    {
        if (!isPlaying) return;

        visualizer.DrawSliceMarker(value);

        float score = evaluator.Evaluate(value);

        totalScore += score;
        attemptsUsed++;

        Debug.Log($"Attempt {attemptsUsed}/{maxAttempts} | Score: {score}");

        if (attemptsUsed >= maxAttempts)
        {
            EndMinigame();
        }
    }
    private void EndMinigame()
    {
        isPlaying = false;
        sliderMover.StopMoving();

        float finalScore = totalScore;

        var (text, reward) = GetCookingResult(finalScore, ShipManager.shipManager.cookingLevel);
        resultText.gameObject.SetActive(true);
        resultText.text = text;
        ShipManager.shipManager.crewHunger += (reward);

        StartCoroutine(HideAfterDelay());
    }

    public enum CutQuality { Miss, Weak, Good, Perfect }

    CutQuality GetCutQuality(float score, int level)
    {
        float reduction = level * 0.3f; // tweak this

        float perfect = 2.5f - reduction;
        float good = 1.5f - reduction;
        float weak = 0.5f - reduction;

        if (score > perfect)
            return CutQuality.Perfect;
        else if (score > good)
            return CutQuality.Good;
        else if (score > weak)
            return CutQuality.Weak;
        else
            return CutQuality.Miss;
    }
    int GetFoodReward(CutQuality quality, int level)
    {
        int min = 0;
        int max = 0;

        switch (quality)
        {
            case CutQuality.Perfect:
                min = 8; max = 12;
                break;
            case CutQuality.Good:
                min = 5; max = 8;
                break;
            case CutQuality.Weak:
                min = 1; max = 5;
                break;
            case CutQuality.Miss:
                return 0;
        }

        // Scale with level (0–4)
        float t = level / 4f;

        int scaledMin = Mathf.RoundToInt(min * Mathf.Lerp(1f, 1.8f, t));
        int scaledMax = Mathf.RoundToInt(max * Mathf.Lerp(1f, 1.8f, t));

        return Random.Range(scaledMin, scaledMax + 1);
    }

    (string text, int foodAmount) GetCookingResult(float score, int level)
    {
        CutQuality quality = GetCutQuality(score, level);
        int amount = GetFoodReward(quality, level);

        string text = quality switch
        {
            CutQuality.Perfect => $"Perfect Cut! You gained {amount} food.",
            CutQuality.Good => $"Good Cut. You gained {amount} food.",
            CutQuality.Weak => $"Weak Cut. You gained {amount} food.",
            _ => $"Missed Cut. You gained {amount} food."
        };

        return (text, amount);
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        resultText.gameObject.SetActive(false);
        minigameUIRoot.SetActive(false);
        GameplayModeManager.Instance.SetCookingMode(false);
    }

    public void ExitMinigame()
    {
        if (!isPlaying) return;

        isPlaying = false;

        StopAllCoroutines();
        sliderMover.StopMoving();

        resultText.gameObject.SetActive(false);
        minigameUIRoot.SetActive(false);

        GameplayModeManager.Instance.SetCookingMode(false);
    }
}