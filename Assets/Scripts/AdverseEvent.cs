using UnityEngine;
using UnityEngine.UI;

public class AdverseEvent : MonoBehaviour
{
    private float eventDuration = 20;
    private float timer = 20;

    private float fixRate = 1;
    public float fixTime = 10;
    private float currentFixAmount = 10;

    private bool isRunning = true;
    private bool isFixing = false;


    public Image timerBar;
    public Image fixBar;

    public Task thisTask;
    public int taskIndex;

    public void setFields(float eventDuration, float fixRate, float fixTime)
    {
        this.eventDuration = eventDuration;
        this.timer = eventDuration;
        this.fixRate = fixRate;
        this.fixTime = fixTime;
        this.currentFixAmount = fixTime;
    }

    void Update()
    {
        UpdateTimer();
        UpdateFixing();
    }

    private void UpdateTimer()
    {
        timerBar.enabled = false;
        if (!isRunning) return;

        if (currentFixAmount < fixTime)
        {
            fixBar.enabled = true;
            currentFixAmount += 2 * fixRate * Time.deltaTime;
            fixBar.fillAmount = (fixTime - currentFixAmount) / fixTime;
            return;
        }
        else
        {
            fixBar.enabled = false;
            fixTime = currentFixAmount;
        }

        timerBar.enabled = true;
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = 0f;
            isRunning = false;
            OnTimerEnd();
        }

        timerBar.fillAmount = timer / eventDuration;
    }

    private void UpdateFixing()
    {
        fixBar.enabled = currentFixAmount < fixTime;
        if (!isFixing) return;
        fixBar.enabled = true;
        currentFixAmount -= fixRate * ShipManager.shipManager.repairEfficiencyMultiplier * ShipManager.shipManager.actingEfficiencyMultiplier * Time.deltaTime;

        if (currentFixAmount <= 0f)
        {
            isRunning = false;
            OnFixed();
        }

        fixBar.fillAmount = (fixTime - currentFixAmount) / fixTime;
    }

    public void BeginFixing()
    {
        isRunning = false;
        isFixing = true;
    }

    public void StopFixing()
    {
        isRunning = true;
        isFixing = false;
    }
    void OnTimerEnd()
    {
        Destroy(gameObject);
        if (TutorialTaskManager.Instance != null)
        {
            TutorialTaskManager.Instance.activeTaskIndices.Remove(taskIndex);
            TutorialTaskManager.Instance.activeTasks.Remove(thisTask);
            return;
        }
        TaskManager.taskManagerSingleton.activeTaskIndices.Remove(taskIndex);
        TaskManager.taskManagerSingleton.activeTasks.Remove(thisTask);

        ShipManager.shipManager.TakeDamage(5);
    }

    void OnFixed()
    {
        Destroy(gameObject);
        if (TutorialTaskManager.Instance != null)
        {
            TutorialTaskManager.Instance.activeTaskIndices.Remove(taskIndex);
            TutorialTaskManager.Instance.activeTasks.Remove(thisTask);
            return;
        }
        TaskManager.taskManagerSingleton.activeTasks.Remove(thisTask);
        TaskManager.taskManagerSingleton.activeTaskIndices.Remove(taskIndex);

        ShipManager.shipManager.gold += 20;
    }
}
