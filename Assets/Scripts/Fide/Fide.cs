using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fide : MonoBehaviour
{
    [SerializeField] private CanvasGroup background;
    [SerializeField] private CanvasGroup text1;
    [SerializeField] private CanvasGroup text2;
    [SerializeField] private CanvasGroup text3;
    [SerializeField] private CanvasGroup text4;
    [SerializeField] private GameObject objectiveText;

    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private float waitTime = 1f;

    public void FadeIn()
    {
        StartCoroutine(WinningSequence());
    }

    private IEnumerator WinningSequence()
    {
        objectiveText.SetActive(false);
        
        // Background fades in
        yield return StartCoroutine(Fade(background, 0f, 1f));

        // Text 1 fades in
        yield return StartCoroutine(Fade(text1, 0f, 1f));

        yield return new WaitForSecondsRealtime(waitTime);

        // Text 1 fades out
        yield return StartCoroutine(Fade(text1, 1f, 0f));

        // Text 2 fades in
        yield return StartCoroutine(Fade(text2, 0f, 1f));

        yield return new WaitForSecondsRealtime(waitTime);

        // Text 2 fades out
        yield return StartCoroutine(Fade(text2, 1f, 0f));

        // Text 3 fades in
        yield return StartCoroutine(Fade(text3, 0f, 1f));

        yield return new WaitForSecondsRealtime(waitTime);

        // Text 3 fades out
        yield return StartCoroutine(Fade(text3, 1f, 0f));

        // Text 4 fades in
        yield return StartCoroutine(Fade(text4, 0f, 1f));

        yield return new WaitForSecondsRealtime(waitTime);

        // Text 4 fades out
        yield return StartCoroutine(Fade(text4, 1f, 0f));

        Time.timeScale = 1.0f;

        SceneManager.LoadScene("Mainmenu");
    }

    private IEnumerator Fade(CanvasGroup target, float start, float end)
    {
        float time = 0f;

        while (time < fadeTime)
        {
            time += Time.unscaledDeltaTime;
            target.alpha = Mathf.Lerp(start, end, time / fadeTime);

            yield return null;
        }

        target.alpha = end;
    }
}
