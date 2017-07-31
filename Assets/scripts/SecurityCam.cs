using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCam : MonoBehaviour, IHackable
{
    private enum State
    {
        Patrol,
        Off,
        Detect
    };

    [SerializeField]
    private Transform lightCone;
    [SerializeField]
    private float lightRange = 5f;
    [SerializeField]
    private float lightAngle = 45f;

    [Space]
    [SerializeField, Range(0, 360)]
    private float patrolAngle = 180f;
    [SerializeField]
    private float patrolSpeed = 1f;
    [SerializeField]
    private float patrolDistance = 1f;

    private State currentState = State.Patrol;
    private float stateTime = 0f;

    private Transform cam;
    private Transform patrolTarget;
    private Vector3 patrolStartPosition;

    new private Light light;
    private Material material;

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
        switch (currentState)
        {
            case State.Patrol:
                PatrolBehaviour();
                break;
            case State.Detect:
                DetectBehaviour();
                break;
            case State.Off:
                OffBehaviour();
                break;
        }
    }

    private void PatrolBehaviour()
    {
        // Move target to make camera rotate
        patrolTarget.localPosition = patrolStartPosition +
            patrolDistance * (Mathf.Cos(Mathf.PI * stateTime - 0.5f * Mathf.PI) * Vector3.right +
                Mathf.Sin(Mathf.PI * stateTime - 0.5f * Mathf.PI) * Vector3.forward);

        cam.LookAt(patrolTarget, Vector3.up);

        light.intensity = Mathf.Lerp(light.intensity, 1, 2f * Time.deltaTime);

        // Cast random rays to check for player
        RaycastHit hit;

        for (int i = 0; i < 200; i++)
        {
            Vector3 direction = ConeVector(cam.forward, lightAngle);

            Ray ray = new Ray(transform.position, direction);

            if (Physics.Raycast(ray, out hit, lightRange) && hit.collider.tag == "Player")
            {
                currentState = State.Detect;
                break;
            }
        }

        // Move time smoothly up and down
        float d = 1 - (patrolAngle / 360);
        stateTime = (1 - d) * Mathf.Sin(patrolSpeed * Time.time) + d;
    }

    private void DetectBehaviour()
    {
        // Move target to player
        patrolTarget.position = Vector3.Lerp(patrolTarget.position,
            GameManager.Player.transform.position,
            5f * Time.deltaTime);

        cam.LookAt(patrolTarget, Vector3.up);

        light.intensity = Mathf.Lerp(light.intensity, 10, 2f * Time.deltaTime);

        stateTime += Time.deltaTime;

        // Send game over signal after a short time
        if (stateTime >= 2f && GameManager.CurrentState == GameManager.State.Game)
        {
            GameManager.GameOver();
        }
    }

    private void OffBehaviour()
    {
        patrolTarget.position = Vector3.Lerp(patrolTarget.position,
            transform.position + Vector3.down, Time.deltaTime);

        cam.LookAt(patrolTarget, Vector3.up);

        light.intensity = Mathf.Lerp(light.intensity, 0, 2f * Time.deltaTime);

        stateTime += Time.deltaTime;

        /*if (stateTime >= 10f && GameManager.CurrentState == GameManager.State.Game)
        {
            SetState(State.Patrol);
        }*/
    }

    private void SetState(State newState)
    {
        State oldState = currentState;

        currentState = newState;
        stateTime = 0f;

        if (newState == State.Off)
        {
            //TODO: Turn off lamps
        }

        if (oldState == State.Off)
        {
            //TODO: Turn on lamps
        }
    }

    /// <summary>
    /// Sample a random unit vector from the cone defined by direction and theta
    /// </summary>
    private Vector3 ConeVector(Vector3 direction, float theta)
    {
        float z = Random.Range(Mathf.Cos(Mathf.Deg2Rad * lightAngle / 2), 1);
        float phi = Random.Range(0, 2 * Mathf.PI);

        float z_ = Mathf.Sqrt(1 - Mathf.Pow(z, 2));
        float x = z_ * Mathf.Cos(phi);
        float y = z_ * Mathf.Sin(phi);

        Vector3 vector = new Vector3(x, y, z);
        // Rotate the random vector in the direction space
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, direction);
        vector = rotation * vector;

        return vector;
    }

    public void Hack()
    {
        SetState(State.Off);
    }

    public void Reset()
    {
        SetState(State.Patrol);
    }


    private void OnDrawGizmos()
    {
        //Gizmos.matrix = transform.localToWorldMatrix;
        //Vector3 last = patrolDistance * (Mathf.Cos(Mathf.PI * 0 - 0.5f * Mathf.PI) * Vector3.right + Mathf.Sin(Mathf.PI * 0 - 0.5f * Mathf.PI) * Vector3.forward);

        //float d = 1 - (patrolAngle / 360);
        //stateTime = (1 - d) * Mathf.Sin(patrolSpeed * Time.time) + d;

        //for (float i = 0; i < 2f; i += 0.1f)
        //{
        //    stateTime += i;
        //    Vector3 newVec = patrolDistance * (Mathf.Cos(Mathf.PI * stateTime - 0.5f * Mathf.PI) * Vector3.right + Mathf.Sin(Mathf.PI * stateTime - 0.5f * Mathf.PI) * Vector3.forward);
        //    Gizmos.DrawLine(last, newVec);
        //    last = newVec;
        //}

    }
}
