using UnityEngine;
using UnityEngine.UI;

public class BucketScript : MonoBehaviour
{
    private float fillRate = 1;
    private float fillTime = 10;
    private float currentFillAmount = 10;

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
        GetComponent<Renderer>().material.color = Color.blue; // Change color to indicate it's filled
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
