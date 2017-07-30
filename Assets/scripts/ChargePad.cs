using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePad : MonoBehaviour
{
    private AudioSource audio;

    private bool player = false;

    protected void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    protected void Update()
    {
        if (player && GameManager.Player.Battery < GameManager.Player.MaxCharge)
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }
        }
        else if (audio.isPlaying)
        {
            //audio.Stop();
        }
    }

    protected void OnTriggerEnter(Collider col)
    {
        GameManager.Player.IsCharging = true;
        GameManager.Checkpoint = this;
        player = true;
    }

    protected void OnTriggerExit(Collider col)
    {
        GameManager.Player.IsCharging = false;
        player = false;
    }
}
