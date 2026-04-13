using UnityEngine;
using UnityEngine.UI;

public class BucketScript : MonoBehaviour
{
    [SerializeField] private float fillRate = 1;
    [SerializeField] private float fillTime = 5;
    private float currentFillAmount = 5;

    public Image fillBar;

    bool isFilling = false;
    public bool isFilled = false;

    [SerializeField] GameObject waterObj;

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
        if (currentFillAmount == 0f) 
        {
            OnEmpty();
        }

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
        waterObj.SetActive(true);
    }

    private void OnEmpty() 
    {
        isFilled = false;
        waterObj.SetActive(true);
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
