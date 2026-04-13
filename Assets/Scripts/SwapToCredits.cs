using UnityEngine;
using System.Collections;

public class SwapToCredits : MonoBehaviour
{
    [SerializeField] private RectTransform creditsImage;
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private float startY = -800f;
    [SerializeField] private float endY = 800f;

    private void StartCredits()
    {
        StartCoroutine(ScrollCredits());
    }

    IEnumerator ScrollCredits()
    {
        // Start position (off-screen bottom)
        creditsImage.anchoredPosition = new Vector2(
            creditsImage.anchoredPosition.x,
            startY
        );

        // Scroll upward
        while (creditsImage.anchoredPosition.y < endY)
        {
            creditsImage.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
            yield return null;
        }

        OnCreditsFinished();
    }

    void OnCreditsFinished()
    {
        Debug.Log("Credits finished");

        // Optional: load scene or return to menu
        // SceneManager.LoadScene("MainMenu");
    }
}
