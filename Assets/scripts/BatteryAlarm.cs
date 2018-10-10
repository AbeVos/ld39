using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryAlarm : MonoBehaviour
{
    [SerializeField]
    private float alarmThreshold = 5f;
    [SerializeField]
    private float powerDownThreshold = 3f;

    private Player player;
    private AudioSource audio;

    protected void Awake()
    {
        player = GetComponentsInParent<Player>()[0];
        audio = GetComponent<AudioSource>();
    }

    protected void Update()
    {
        if (player.Battery < alarmThreshold && !player.IsCharging)
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }

            if (player.Battery < powerDownThreshold)
            {
                float x = player.Battery / powerDownThreshold;
                float pitch  = x * Mathf.Exp(x) / Mathf.Exp(1);

                audio.pitch = pitch;
            }
            else
            {
                audio.pitch = 1f;
            }
        }
        else if (audio.isPlaying)
        {
            audio.Stop();
        }
    }
}
