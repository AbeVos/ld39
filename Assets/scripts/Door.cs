using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IHackable
{
    private Vector3 startPosition;
    private Vector3 endPosition;

    private bool isOpening = false;
    private float t = 0f;

    protected void Awake()
    {
        startPosition = transform.position;
        endPosition = startPosition + 1.5f * transform.right;
    }

    protected void Update()
    {
        transform.position = Vector3.Lerp(startPosition, endPosition,
            (1 - Mathf.Cos(Mathf.PI * t)) / 2f);

        if (isOpening && t < 1f)
        {
            t += Time.deltaTime / 2;

            if (t > 1f)
                t = 1f;
        }
        else if (!isOpening && t > 0f)
        {
            t -= Time.deltaTime / 2;

            if (t < 0f)
                t = 0f;
        }
    }

    public void Open()
    {
        isOpening = true;
    }

    public void Close()
    {
        isOpening = false;
    }

    public void Hack()
    {
        Open();
    }
}
