using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePad : MonoBehaviour
{
    protected void OnTriggerEnter(Collider col)
    {
        GameManager.Player.IsCharging = true;
    }

    protected void OnTriggerExit(Collider col)
    {
        GameManager.Player.IsCharging = false;
    }
}
