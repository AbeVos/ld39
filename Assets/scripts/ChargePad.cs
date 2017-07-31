using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePad : MonoBehaviour
{
    [SerializeField]
    private Color emissionColor = Color.yellow;

    private AudioSource audio;
    private Material material;

    private bool player = false;

    protected void Awake()
    {
        audio = GetComponent<AudioSource>();
        material = GetComponentInChildren<Renderer>().material;
    }

    protected void Update()
    {
        if (player && GameManager.Player.Battery < GameManager.Player.MaxCharge)
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }

            material.SetColor("_EmissionColor",
                Color.Lerp(material.GetColor("_EmissionColor"), 
                    (1 - Mathf.Sin(2f * Mathf.PI * Time.time)) / 2 * emissionColor,
                    2f * Time.deltaTime));
        }
        else
        {
            material.SetColor("_EmissionColor",
                Color.Lerp(material.GetColor("_EmissionColor"), Color.black,
                    2f * Time.deltaTime));
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
