using UnityEngine;
using UnityEngine.UI;

public class AdverseEvent : MonoBehaviour
{
    private float eventDuration = 20;
    private float timer = 20;

    private float fixRate = 1;
    private float fixTime = 10;
    private float currentFixAmount = 10;

    private bool isRunning = true;
    private bool isFixing = false;


    public Image timerBar;
    public Image fixBar;

    public Task thisTask;

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
        currentFixAmount -= fixRate * Time.deltaTime;

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
        Debug.Log("Timer finished!");
        TaskManager.taskManagerSingleton.activeTasks.Remove(thisTask);
    }

    void OnFixed()
    {
        Debug.Log("Event fixed!");
        GameObject.Destroy(gameObject);
        TaskManager.taskManagerSingleton.activeTasks.Remove(thisTask);
    }
}
