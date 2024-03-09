using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    PlayerControls inputActions;

    public float movement;

    private InputAction _moveInputAction, _jumpInputAction;

    public bool jumpFlag;
    public bool isJumpPerforming;

    private void Awake()
    {
        inputActions = new PlayerControls();
    }

    private void OnEnable()
    {
        _moveInputAction = inputActions.Player.Movement;
        _jumpInputAction = inputActions.Player.Jump;

        inputActions.Player.Jump.performed += OnJump;

        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Jump.performed -= OnJump;

        inputActions.Player.Disable();
    }

    public void TickInput()
    {
        MoveInput();
        JumpInput();
    }

    private void MoveInput()
    {
        movement = _moveInputAction.ReadValue<float>();
    }

    private void JumpInput()
    {
        isJumpPerforming = _jumpInputAction.phase == UnityEngine.InputSystem.InputActionPhase.Performed ? true : false;
    }

    private void Update()
    {
        TickInput();
    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        jumpFlag = true;
    }
}
