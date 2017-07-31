using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private Text text;

    private ScreenFader fader;

    private Color textInvisible = new Color(1, 1, 1, 0);
    private Color textOpaque = Color.white;

    protected void Awake()
    {
        Cursor.lockState = CursorLockMode.None;

        fader = FindObjectOfType<ScreenFader>();
        text.color = textInvisible;
    }

    protected void Start()
    {
        fader.FadeIn(2f, ShowText);
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
        {
            fader.FadeOut(2f, StartGame);
        }
    }

    private void ShowText()
    {
        StartCoroutine(ShowTextCoroutine());
    }

    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private IEnumerator ShowTextCoroutine()
    {
        float t = 0f;

        while (true)
        {
            text.color = Color.Lerp(textInvisible, textOpaque,
                (Mathf.Cos(2f * Mathf.PI * t + Mathf.PI) + 1) / 2f);

            t += 0.5f * Time.deltaTime;

            yield return new WaitForEndOfFrame();

            if (t > 1f)
            {
                t -= 1f;
            }
        }
    }
}
