using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float maxBattery = 10f;
    [SerializeField]
    private float chargeRate = 5f;

    private CharacterController controller;

    private float battery;
    private bool isCharging = false;
    private float chargeValue = 1f;

    public bool IsCharging
    {
        set { isCharging = value; }
    }

    protected void Awake()
    {
        controller = GetComponent<CharacterController>();

        battery = maxBattery;
    }

    protected void Update()
    {
        Vector3 inputDirection = Input.GetAxis("Horizontal") * transform.right + 
            Input.GetAxis("Vertical") * transform.forward;

        controller.SimpleMove(inputDirection.normalized);

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.EndGame();
        }

        if (isCharging)
        {
            battery += Time.deltaTime * chargeRate;

            if (battery > maxBattery)
            {
                battery = maxBattery;
            }
            else
            {
                //TODO: Add charging feedback
            }    
        }
        else
        {
            battery -= Time.deltaTime;
        }
    }

    protected void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 40), "Battery: " + battery);
    }
}
