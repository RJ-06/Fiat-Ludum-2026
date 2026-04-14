using UnityEngine;
using System.Collections;

public class SwapToCredits : MonoBehaviour
{
    [SerializeField] private RectTransform creditsImage;
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private float startY = -800f;
    [SerializeField] private float endY = 800f;

    [SerializeField] private Animator animator;

    private void StartCredits()
    {
        animator.enabled = false;
        StartCoroutine(ScrollCredits());
    }

    IEnumerator ScrollCredits()
    {
        Debug.Log("Credits started");

        creditsImage.anchoredPosition = new Vector2(
            creditsImage.anchoredPosition.x,
            startY
        );

        while (creditsImage.anchoredPosition.y < endY)
        {
            creditsImage.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
            yield return null;
        }

        creditsImage.anchoredPosition = new Vector2(
            creditsImage.anchoredPosition.x,
            endY
        );

        Debug.Log("Credits finished - coroutine ended");

        yield break;
    }
}
