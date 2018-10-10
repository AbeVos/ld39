using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    private Image faderImage;

    [SerializeField]
    private Color opaque = Color.black;
    [SerializeField]
    private Color invisible = new Color(0,0,0,0);

    protected void Awake()
    {
        faderImage = GetComponent<Image>();
    }

    public void FadeIn(float time, Action callback)
    {
        StartCoroutine(FadeInCoroutine(time, callback));
    }

    public void FadeOut(float time, Action callback)
    {
        StartCoroutine(FadeOutCoroutine(time, callback));
    }

    private IEnumerator FadeInCoroutine(float time, Action callback)
    {
        faderImage.color = opaque;

        float t = 0;

        while (true)
        {
            float theta = t * Mathf.Exp(t) / Mathf.Exp(1);

            faderImage.color = Color.Lerp(opaque, invisible, theta);

            t += Time.deltaTime / time;

            yield return new WaitForEndOfFrame();

            if (t >= 1f) break;
        }

        faderImage.color = invisible;

        if (callback != null)
            callback();
    }

    private IEnumerator FadeOutCoroutine(float time, Action callback)
    {
        faderImage.color = invisible;

        float t = 0;

        while (t < 1f)
        {
            float theta = t * Mathf.Exp(t) / Mathf.Exp(1);

            faderImage.color = Color.Lerp(invisible, opaque, theta);

            t += Time.deltaTime / time;

            yield return new WaitForEndOfFrame();
        }

        faderImage.color = opaque;

        if (callback != null)
            callback();
    }
}
