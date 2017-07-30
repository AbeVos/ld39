using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float selectionRange = 1f;
    [SerializeField]
    private float speed = 1f;

    [Space]
    [SerializeField]
    private float maxBattery = 10f;
    [SerializeField]
    private float chargeRate = 5f;

    new private Camera camera;
    private CharacterController controller;
    private AudioSource audio;
    private Tooltip tooltip;

    private Vector3 velocity = Vector3.zero;

    private float battery;
    private bool isCharging = false;

    private bool showingText = false;

    public float Battery
    {
        get { return battery; }
    }

    public bool IsCharging
    {
        get { return isCharging; }
        set { isCharging = value; }
    }

    public float MaxCharge
    {
        get { return maxBattery; }
    }

    protected void Awake()
    {
        camera = GetComponentInChildren<Camera>();
        controller = GetComponent<CharacterController>();
        audio = GetComponent<AudioSource>();
        tooltip = FindObjectOfType<Tooltip>();

        battery = maxBattery;
    }

    protected void Update()
    {
        if (GameManager.CurrentState != GameManager.State.Game)
            return;

        // Movement
        Vector3 inputDirection = Mathf.Round(Input.GetAxis("Horizontal")) * transform.right + 
            Mathf.Round(Input.GetAxis("Vertical")) * transform.forward;

        velocity = Vector3.Lerp(velocity, speed * inputDirection.normalized,
            5f * Time.deltaTime);

        controller.SimpleMove(velocity);
        if (controller.velocity.sqrMagnitude > 0.0001f)
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }
            else
                audio.pitch = Mathf.Clamp(controller.velocity.magnitude, 0, 10);
        }
        else
        {
            if (audio.isPlaying)
                audio.Stop();
        }

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
