using TMPro;
using UnityEngine;
using System.Collections;

public class TextFade : MonoBehaviour
{
    public TMP_Text text;
    public float fadeDuration = 1f;

    public void Show(string message)
    {
        text.text = message;
        StopAllCoroutines();
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        Color c = text.color;

        // Fade in
        for (float t = 0; t < 1; t += Time.deltaTime / fadeDuration)
        {
            text.color = new Color(c.r, c.g, c.b, t);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // Fade out
        for (float t = 1; t > 0; t -= Time.deltaTime / fadeDuration)
        {
            text.color = new Color(c.r, c.g, c.b, t);
            yield return null;
        }

        Destroy(gameObject);
    }
}