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

        totalScore += score + 0.3f * ShipManager.shipManager.cookingLevel;
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

        string reward = GetReward(finalScore);

        resultText.gameObject.SetActive(true);
        resultText.text = $"Score: {finalScore:0.00}. You obtained: {reward}";

        StartCoroutine(HideAfterDelay());
    }

    private string GetReward(float score)
    {
        if (score > 2.5f)
            return "Perfect Cut";
        else if (score > 1.5f)
            return "Good Cut";
        else if (score > 0.5f)
            return "Weak Cut";
        else
            return "Nothing";
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        resultText.gameObject.SetActive(false);
        minigameUIRoot.SetActive(false);
    }
}