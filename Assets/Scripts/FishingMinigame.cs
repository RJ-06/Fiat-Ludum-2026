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

    [SerializeField] Image canCatchNotification;
    [SerializeField] Image failedCatchNotification;

    public void FixedUpdate()
    {
        if (canCatch) 
        {
            timer += Time.fixedDeltaTime;
        }
    }

    public IEnumerator StartFishing()
    {
        score = 0;
        timer = 0;
        canCatchNotification.enabled = false;
        failedCatchNotification.enabled = false;
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
        canCatchNotification.enabled = false;
        canCatch = false;
        timer = 0;
        if(score == 0)failedCatchNotification.enabled = true;
        yield return new WaitForSeconds(1f);
        failedCatchNotification.enabled = false;
        GameplayModeManager.Instance.SetFishingMode(false);
    }

    public void tryToCatch()
    {
        if (canCatch)
        {
            canCatch = false;
            canCatchNotification.enabled = false;
            score = 1 - timer / timeToCatch;
            timer = 0;
            GameplayModeManager.Instance.SetFishingMode(false);
            Debug.Log(score);
        }
    }

}
