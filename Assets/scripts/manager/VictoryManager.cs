using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour
{
    [SerializeField]
    private Texture[] images;

    private RawImage rawImage;
    private int currentIdx;

    protected void Awake()
    {
        Cursor.lockState = CursorLockMode.None;

        rawImage = GetComponent<RawImage>();

        //rawImage.texture = images[Random.Range(0, images.Length)];
        StartCoroutine(ImageSwapCoroutine());
    }

    protected void Update()
    {
        rawImage.rectTransform.localScale += 0.01f * Time.deltaTime * Vector3.one;
    }

    private IEnumerator ImageSwapCoroutine()
    {
        float t = 0f;
        currentIdx = Random.Range(0, images.Length);

        while (t < 12f)
        {
            float wait = 1 - 1 / (1 + Mathf.Exp(-0.5f * t + 3));

            yield return new WaitForSeconds(wait);

            int newIdx = Random.Range(0, images.Length);
            while (newIdx == currentIdx)
            {
                newIdx = Random.Range(0, images.Length);
            }
            currentIdx = newIdx;
            rawImage.texture = images[currentIdx];

            t += wait;
        }

        rawImage.texture = null;
        rawImage.color = Color.black;
    }
}
