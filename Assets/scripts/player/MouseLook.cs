using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private Transform horizontalRotator;
    [SerializeField]
    private Transform verticalRotator;

    [SerializeField]
    private Vector2 speed = new Vector2(10f, 10f);
    [SerializeField]
    private float lerpSpeed = 5f;
    [SerializeField]
    private Vector2 xRange = new Vector2(-360, 360);
    [SerializeField]
    private Vector2 yRange = new Vector2(-60, 60);

    private float rotationX = 0F;
    private float rotationY = 0F;


    protected void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    protected void Update ()
    {
        // Switch cursor lockstates
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (GameManager.CurrentState != GameManager.State.Game)
            return;

        // Read the mouse input axis
        rotationX += Input.GetAxis("Mouse X") * speed.x;
        rotationY += Input.GetAxis("Mouse Y") * speed.y;

        rotationX = ClampAngle (rotationX, xRange.x, xRange.y);
        rotationY = ClampAngle (rotationY, yRange.x, yRange.y);

        Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, -Vector3.right);

        horizontalRotator.localRotation = 
            Quaternion.SlerpUnclamped(horizontalRotator.localRotation,
                Quaternion.identity * xQuaternion, lerpSpeed * Time.deltaTime);
        
        verticalRotator.localRotation = 
            Quaternion.SlerpUnclamped(verticalRotator.localRotation,
                Quaternion.identity * yQuaternion, lerpSpeed * Time.deltaTime);
    }

    private float ClampAngle (float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp (angle, min, max);
    }
}
