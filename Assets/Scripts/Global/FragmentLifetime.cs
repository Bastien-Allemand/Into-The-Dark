using System.Collections;
using UnityEngine;

public class FragmentLifetime : MonoBehaviour
{
    [HideInInspector] public float lifetime = 5f;
    [HideInInspector] public float fadeDuration = 1f;

    void Start()
    {
        StartCoroutine(ShrinkAndDestroy());
    }

    private IEnumerator ShrinkAndDestroy()
    {
        yield return new WaitForSeconds(lifetime);

        Vector3 originalScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, elapsedTime / fadeDuration);
            yield return null;
        }

        Destroy(gameObject);
    }
}