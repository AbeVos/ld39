using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    private ScreenFader fader;
    private Text text;

    private bool introRunning = false;

    protected void Awake()
    {
        fader = GetComponentInChildren<ScreenFader>();
        text = GetComponentInChildren<Text>();
    }

    protected void Start()
    {
        fader.FadeIn(3f, StartIntro);
    }

    protected void Update()
    {
        if (introRunning)
        {
            text.rectTransform.position += 30f * Time.deltaTime * Vector3.up;
        
            if (text.rectTransform.position.y >= 750f || Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                fader.FadeOut(3f, StartGame);
            }
        }
    }

    private void StartIntro()
    {
        introRunning = true;
    }

    private void StartGame()
    {
        SceneManager.LoadScene(2);
    }
}
