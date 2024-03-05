using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    PlayerControls inputActions;

    public float movement;

    public bool jumpFlag;

    private InputAction _moveInputAction;

    private void Awake()
    {
        inputActions = new PlayerControls();
    }

    private void OnEnable()
    {
        _moveInputAction = inputActions.Player.Movement;

        inputActions.Player.Jump.performed += OnJump;

        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Jump.performed -= OnJump;

        inputActions.Player.Disable();
    }

    private void Update()
    {
        movement = _moveInputAction.ReadValue<float>();
    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        jumpFlag = true;
    }
}
