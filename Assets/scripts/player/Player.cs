using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float selectionRange = 1f;

    [Space]
    [SerializeField]
    private float maxBattery = 10f;
    [SerializeField]
    private float chargeRate = 5f;

    new private Camera camera;
    private CharacterController controller;

    private float battery;
    private bool isCharging = false;

    public bool IsCharging
    {
        set { isCharging = value; }
    }

    protected void Awake()
    {
        camera = GetComponentInChildren<Camera>();
        controller = GetComponent<CharacterController>();

        battery = maxBattery;
    }

    protected void Update()
    {
        if (GameManager.CurrentState != GameManager.State.Game)
            return;

        // Movement
        Vector3 inputDirection = Input.GetAxis("Horizontal") * transform.right + 
            Input.GetAxis("Vertical") * transform.forward;

        controller.SimpleMove(inputDirection.normalized);

        // Battery management
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

            if (battery < 0f)
            {
                GameManager.GameOver();
            }
        }

        // Activatables
        RaycastHit hit;

        if (Physics.Raycast(camera.transform.position, camera.transform.forward,
            out hit, selectionRange))
        {
            IActivatable activatable = hit.collider.GetComponent<IActivatable>();
            if (activatable != null && Input.GetMouseButtonDown(0))
            {
                activatable.Activate();
            }
        }
    }

    protected void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 40), "Battery: " + battery);
    }

    public void Checkpoint()
    {
        Debug.Log("Change position");
        transform.position = GameManager.Checkpoint.transform.position;
        transform.rotation = GameManager.Checkpoint.transform.rotation;
    }
}
