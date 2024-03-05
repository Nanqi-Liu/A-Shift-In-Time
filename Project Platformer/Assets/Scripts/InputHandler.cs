using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    PlayerControls inputActions;

    public float moveLeft;
    public float moveRight;

    public bool jumpFlag;

    private void Awake()
    {
        inputActions = new PlayerControls();
    }
}
