using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StarAnimator : MonoBehaviour
{
    [SerializeField]
    private float enlargeScale = 1.5f;

    [SerializeField]
    private float shrinkScale = 1f;

    [SerializeField]
    private float enlargeDuration = 0.25f;

    [SerializeField]
    private float shrinkDuration = 0.25f;

    [SerializeField]
    private Image[] stars;

    public void ShowStars(int numStars)
    {
        StartCoroutine(ShowStarsRoutine(numStars));
    }

    private IEnumerator ShowStarsRoutine(int numStars)
    {
        foreach (Image star in stars)
            star.transform.localScale = Vector3.zero;

        for (int i = 0; i < numStars; i++)
            yield return StartCoroutine(EnlargeAndShrinkStar(stars[i]));
    }

    private IEnumerator EnlargeAndShrinkStar(Image star)
    {
        yield return StartCoroutine(ChangeStarScale(star, enlargeScale, enlargeDuration));
        yield return StartCoroutine(ChangeStarScale(star, shrinkScale, shrinkDuration));
    }

    private IEnumerator ChangeStarScale(Image star, float targetScale, float duration)
    {
        Vector3 initialScale = star.transform.localScale;
        Vector3 finalScale = new(targetScale, targetScale, targetScale);

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            star.transform.localScale = Vector3.Lerp(
                initialScale,
                finalScale,
                elapsedTime / duration
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        star.transform.localScale = finalScale;
    }
}
