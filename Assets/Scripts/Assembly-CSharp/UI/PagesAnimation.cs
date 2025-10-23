using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PagesAnimation : MonoBehaviour
{
    [SerializeField] private Image[] pageImages;

    [Header("Options")]
    [SerializeField][Range(0, 255)] private int maxAlpha = 255;
    [SerializeField] private float fadeInDuration = 2f;
    [SerializeField] private float fadeOutDuration = 2f;
    [SerializeField] private float overlapTime = 1f;

    private void Start()
    {
        foreach (var page in pageImages)
        {
            if (page != null)
            {
                var c = page.color;
                c.a = 0f;
                page.color = c;
            }
        }

        if (pageImages.Length > 0)
        {
            StartCoroutine(AnimatePages());
        }
    }

    private IEnumerator AnimatePages()
    {
        int index = 0;
        float targetAlpha = maxAlpha / 255f;

        Image current = pageImages[index];
        yield return StartCoroutine(FadeImage(current, 0f, targetAlpha, fadeInDuration));

        while (true)
        {
            // Image actuelle et suivante
            Image next = pageImages[(index + 1) % pageImages.Length];

            // Start fade out on current image
            Coroutine fadeOut = StartCoroutine(FadeImage(current, targetAlpha, 0f, fadeOutDuration));

            // Wait before starting next image fade in
            yield return new WaitForSeconds(fadeOutDuration - overlapTime);

            // Start fade in for next image and fade out for current image
            yield return StartCoroutine(FadeImage(next, 0f, targetAlpha, fadeInDuration));

            // Wait until fade out completed
            yield return fadeOut;

            // Next image
            current = next;
            index = (index + 1) % pageImages.Length;
        }
    }

    private IEnumerator FadeImage(Image img, float startAlpha, float endAlpha, float duration)
    {
        float t = 0f;
        Color c = img.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            img.color = c;
            yield return null;
        }

        c.a = endAlpha;
        img.color = c;
    }
}
