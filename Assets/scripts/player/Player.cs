using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;

    protected void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    protected void Update()
    {
        Vector3 inputDirection = Input.GetAxis("Horizontal") * transform.right + 
            Input.GetAxis("Vertical") * transform.forward;

        controller.SimpleMove(inputDirection);

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.EndGame();
        }
    }
}
