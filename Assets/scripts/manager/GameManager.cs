using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static Player player;

    public static Player Player
    { get { return player; } }

    protected void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public static void EndGame()
    {
        SceneManager.LoadScene(0);
    }
}
