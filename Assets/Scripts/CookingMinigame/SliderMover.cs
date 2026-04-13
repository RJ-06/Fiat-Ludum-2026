using UnityEngine;

public class SliderMover : MonoBehaviour
{
    [Header("Bounds")]
    public float leftBound = -300f;
    public float rightBound = 300f;

    [Header("Speed")]
    public float speed = 2f;

    private float currentValue;
    private bool isMoving;

    public float CurrentValue => currentValue;

    [SerializeField] private RectTransform sliderRect;

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    void Update()
    {
        if (!isMoving) return;

        float t = Mathf.PingPong(Time.time * (speed - 0.3f * ShipManager.shipManager.cookingLevel), 1f);
        currentValue = Mathf.Lerp(leftBound, rightBound, t);

        UpdateVisual();
    }

    void UpdateVisual()
    {
        Vector2 pos = sliderRect.anchoredPosition;
        pos.x = currentValue;
        sliderRect.anchoredPosition = pos;
    }
}