using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        FadeIn,
        Game,
        GameOver,
        FadeOut
    }

    private static ScreenFader fader;
    private static Player player;

    private static State currentState = State.FadeIn;

    public static State CurrentState
    { get { return currentState; } }

    public static Player Player
    { get { return player; } }

    protected void Awake()
    {
        fader = FindObjectOfType<ScreenFader>();
        player = FindObjectOfType<Player>();
    }

    protected void Start()
    {
        fader.FadeIn(1f, StartGame);
    }

    private static void SetState(State newState)
    {
        currentState = newState;
    }

    public static void GameOver()
    {
        SetState(State.FadeOut);
        fader.FadeOut(3f, EndGame);
    }

    private void StartGame()
    {
        Debug.Log("Start game");
        SetState(State.Game);
    }

    private static void EndGame()
    {
        SceneManager.LoadScene(0);
    }
}
