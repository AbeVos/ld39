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
    private Tooltip tooltip;

    private float battery;
    private bool isCharging = false;

    private bool showingText = false;

    public bool IsCharging
    {
        set { isCharging = value; }
    }

    protected void Awake()
    {
        camera = GetComponentInChildren<Camera>();
        controller = GetComponent<CharacterController>();
        tooltip = FindObjectOfType<Tooltip>();

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
            Debug.Log(hit.collider.name);
            IActivatable activatable = hit.collider.GetComponent<IActivatable>();
            if (activatable != null)
            {
                if (!showingText)
                {
                    showingText = true;
                    tooltip.ShowText(activatable.TooltipMessage, -1);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    activatable.Activate();
                }
            }
            else if (showingText)
            {
                showingText = false;
                tooltip.HideText();
            }
        }
        else if (showingText)
        {
            showingText = false;
            tooltip.HideText();
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
