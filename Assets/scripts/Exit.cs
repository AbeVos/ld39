using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour, IActivatable
{
    [SerializeField]
    private int sceneIndex = 2;
    [SerializeField]
    private string tooltipMessage = "Kutvliegen";

    public string TooltipMessage
    { get { return tooltipMessage; } }

    public void Activate()
    {
        FindObjectOfType<ScreenFader>().FadeOut(2f, ExitLevel);
    }

    private void ExitLevel()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
