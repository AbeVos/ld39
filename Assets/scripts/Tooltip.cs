using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private Text tooltipText;

    [SerializeField]
    private Color opaque = Color.white;
    [SerializeField]
    private Color invisible = new Color(1,1,1,0);

    private bool active = false;

    protected void Awake()
    {
        tooltipText = GetComponent<Text>();
    }

    public void ShowText(string text, float time)
    {
        StopAllCoroutines();

        if (time > 0f)
            StartCoroutine(ShowTextCoroutine(text, time));
        else if (!active)
            StartCoroutine(ShowTextCoroutine(text));
    }

    public void HideText()
    {
        if (active)
        {
            StartCoroutine(HideTextCoroutine());
        }
    }

    private IEnumerator ShowTextCoroutine(string text, float time)
    {
        active = true;

        tooltipText.text = text;
        tooltipText.color = invisible;

        // Fade in
        float t = 0f;
        while (t < 1f)
        {
            tooltipText.color = Color.Lerp(invisible, opaque, t);

            t += 2f * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        tooltipText.color = opaque;

        yield return new WaitForSeconds(time);

        // Fade out
        t = 0f;
        while (t < 1f)
        {
            tooltipText.color = Color.Lerp(opaque, invisible, t);

            t += 2f * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        tooltipText.color = invisible;

        active = false;
    }

    private IEnumerator ShowTextCoroutine(string text)
    {
        active = true;

        tooltipText.text = text;
        tooltipText.color = invisible;

        // Fade in
        float t = 0f;
        while (t < 1f)
        {
            tooltipText.color = Color.Lerp(invisible, opaque, t);

            t += 2f * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        tooltipText.color = opaque;
    }

    private IEnumerator HideTextCoroutine()
    {
        tooltipText.color = opaque;

        // Fade out
        float t = 0f;
        while (t < 1f)
        {
            tooltipText.color = Color.Lerp(opaque, invisible, t);

            t += 2f * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        tooltipText.color = invisible;

        active = false;
    }
}
