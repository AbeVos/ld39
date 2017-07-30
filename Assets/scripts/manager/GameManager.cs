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
    private static ChargePad checkpoint;

    private static State currentState = State.FadeIn;

    public static State CurrentState
    { get { return currentState; } }

    public static Player Player
    { get { return player; } }

    public static ChargePad Checkpoint
    {
        set { checkpoint = value; }
        get { return checkpoint; }
    }

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
        State previousState = currentState;
        currentState = newState;

        Debug.Log("SetState: " + currentState);

        if (previousState == State.FadeOut)
        {
            
        }
    }

    public static void GameOver()
    {
        SetState(State.FadeOut);

        if (checkpoint == null)
        {
            fader.FadeOut(2f, EndGame);
        }
        else
        {
            fader.FadeOut(2f, ReturnToCheckPoint);
        }
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

    private static void ReturnToCheckPoint()
    {
        Debug.Log("Reset cameras");

        foreach (SecurityCam cam in FindObjectsOfType<SecurityCam>())
        {
            cam.Reset();
        }

        player.Checkpoint();

        fader.FadeIn(1f, null);
        SetState(State.Game);
    }
}
