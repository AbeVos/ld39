using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCam : MonoBehaviour
{
    [SerializeField]
    private Transform lightCone;
    [SerializeField]
    private float lightRange = 5f;
    [SerializeField]
    private float lightAngle = 45f;

    private Transform cam;
    private Transform patrolTarget;
    private Vector3 patrolStartPosition;

    private Light light;

    protected void Awake()
    {
        cam = transform.GetChild(0);
        patrolTarget = transform.GetChild(1);
        patrolStartPosition = patrolTarget.localPosition;

        light = GetComponentInChildren<Light>();
        light.range = lightRange;
        light.spotAngle = lightAngle;

        lightCone.localScale = lightRange * Vector3.forward +
            2f * lightRange * Mathf.Tan(0.5f * Mathf.Deg2Rad * lightAngle) *
            (Vector3.right + Vector3.up);
    }

    protected void Update()
    {
        //cam.localEulerAngles = 45f * Mathf.Sin(Time.time) * Vector3.up;

        patrolTarget.localPosition = patrolStartPosition +
            4f * (Mathf.Abs(Mathf.Sin(0.5f * Time.time)) * transform.forward +
                Mathf.Cos(0.5f * Time.time) * transform.right);

        cam.LookAt(patrolTarget, Vector3.up);
    }
}
