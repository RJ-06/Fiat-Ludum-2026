using UnityEngine;
using UnityEngine.UI;

public class AdverseEvent : MonoBehaviour
{
    public float eventDuration;
    private float timer;

    private float fixRate = 1;
    private float fixTime = 10;
    private float currentFixAmount = 10;

    Vector3 coordinates;
    private bool isRunning = true;
    private bool isFixing = false;


    public Image timerBar;
    public Image fixBar;

    void Start()
    {
        timer = eventDuration;
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
    }

    void OnFixed()
    {
        Debug.Log("Event fixed!");
        GameObject.Destroy(gameObject);
    }
}
