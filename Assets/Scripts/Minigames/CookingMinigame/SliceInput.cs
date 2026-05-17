using UnityEngine;

public class SliceInput : MonoBehaviour
{
    [SerializeField] private SliceMinigameController controller;
    [SerializeField] private SliderMover sliderMover;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            controller.OnSlicePressed(sliderMover.CurrentValue);
        }
    }
}