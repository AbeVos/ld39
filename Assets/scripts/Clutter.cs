using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clutter : MonoBehaviour
{
    [SerializeField]
    private Transform[] clutter;

    protected void Awake()
    {
        int idx = Random.Range(0, clutter.Length);
        Instantiate(clutter[idx], transform.position, transform.rotation, transform);
    }
}
