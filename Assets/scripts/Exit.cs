using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField]
    private int sceneIndex = 2;

    protected void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            FindObjectOfType<ScreenFader>().FadeOut(2f, ExitLevel);
        }
    }

    private void ExitLevel()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
