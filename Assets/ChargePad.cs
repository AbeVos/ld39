using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePad : MonoBehaviour
{
    protected void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.name);
    }

    protected void OnTriggerExit(Collider col)
    {
    }
}
