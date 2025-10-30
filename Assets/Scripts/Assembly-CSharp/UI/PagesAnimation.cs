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

    [Header("Random Position")]
    [SerializeField] private bool randomizePosition = true;
    [SerializeField] private Vector2 minPosition = new Vector2(-200f, -200f);
    [SerializeField] private Vector2 maxPosition = new Vector2(200f, 200f);

    [Header("Movement Animation")]
    [SerializeField] private bool enableMovement = true;
    [SerializeField] private float movementDistance = 50f;

    private void Start()
    {
        foreach (var page in pageImages)
        {
            if (page != null)
            {
                var c = page.color;
                c.a = 0f;
                page.color = c;

                if (randomizePosition)
                {
                    RandomizeImagePosition(page);
                }
            }
        }

        if (pageImages.Length > 0)
        {
            StartCoroutine(AnimatePages());
        }
    }

    private void RandomizeImagePosition(Image img)
    {
        RectTransform rectTransform = img.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            float randomX = Random.Range(minPosition.x, maxPosition.x);
            float randomY = Random.Range(minPosition.y, maxPosition.y);
            rectTransform.anchoredPosition = new Vector2(randomX, randomY);
        }
    }

    private IEnumerator AnimatePages()
    {
        int index = 0;
        float targetAlpha = maxAlpha / 255f;

        Image current = pageImages[index];
        Coroutine currentMovement = null;

        if (enableMovement)
        {
            currentMovement = StartCoroutine(MoveImage(current));
        }

        yield return StartCoroutine(FadeImage(current, 0f, targetAlpha, fadeInDuration));

        while (true)
        {
            // Image actuelle et suivante
            Image next = pageImages[(index + 1) % pageImages.Length];

            // Start fade out on current image
            Coroutine fadeOut = StartCoroutine(FadeImage(current, targetAlpha, 0f, fadeOutDuration));

            // Wait before starting next image fade in
            yield return new WaitForSeconds(fadeOutDuration - overlapTime);

            // Start movement for next image
            Coroutine nextMovement = null;
            if (enableMovement)
            {
                nextMovement = StartCoroutine(MoveImage(next));
            }

            // Start fade in for next image and fade out for current image
            yield return StartCoroutine(FadeImage(next, 0f, targetAlpha, fadeInDuration));

            // Wait until fade out completed
            yield return fadeOut;

            // Stop movement of previous image
            if (currentMovement != null)
            {
                StopCoroutine(currentMovement);
            }

            // Next image
            current = next;
            currentMovement = nextMovement;
            index = (index + 1) % pageImages.Length;
        }
    }

    private IEnumerator MoveImage(Image img)
    {
        RectTransform rectTransform = img.GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            yield break;
        }

        Vector2 startPosition = rectTransform.anchoredPosition;

        // Generate a normalized random direction
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));

        Vector2 targetPosition = startPosition + direction * movementDistance;

        float elapsed = 0f;

        while (elapsed < (fadeInDuration + fadeOutDuration))
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (fadeInDuration + fadeOutDuration);
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
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
