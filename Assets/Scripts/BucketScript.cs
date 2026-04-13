using UnityEngine;
using UnityEngine.UI;

public class BucketScript : MonoBehaviour
{
    [SerializeField] private float fillRate = 1;
    [SerializeField] private float fillTime = 3;
    private float currentFillAmount = 5;

    public Image fillBar;

    bool isFilling = false;
    public bool isFilled = false;

    void Update()
    {
        UpdateFilling();
    }

    private void UpdateFilling()
    {
        if (!isFilling) return;
        currentFillAmount -= fillRate * Time.deltaTime;

        if (currentFillAmount <= 0f)
        {
            OnFilled();
        }
        else 
        

            fillBar.fillAmount = (fillTime - currentFillAmount) / fillTime;
    }

    public void BeginFilling()
    {
        isFilling = true;
        fillBar.enabled = true;
    }

    public void StopFilling()
    {
        isFilling = false;
        fillBar.enabled = false;
    }

    private void OnFilled()
    {
        isFilling = false;
        fillBar.enabled = false;
        isFilled = true;
    }

    public void OnEmpty() 
    {
        isFilled = false;
        currentFillAmount = fillTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FillStation") && !isFilled)
        {
            BeginFilling();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("FillStation"))
        {
            StopFilling();
        }
    }
}
