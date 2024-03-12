using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerLocomotion playerLocomotion;
    PlayerDetection playerDetection;
    PlayerParticleHandler playerParticleHandler;

    public bool isFacingRight = true;
    public bool isGrounded;
    public bool isWallSliding;
    public bool isWallJumping;

    [SerializeField] private float _coyoteTime = 0.2f;
    public float coyoteTimeCounter;

    [SerializeField] private float _jumpBufferTime = 0.2f;
    private float _jumpBufferTimeCounter;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerDetection = GetComponent<PlayerDetection>();
        playerParticleHandler = GetComponent<PlayerParticleHandler>();
    }

    private void Start()
    {
        coyoteTimeCounter = 0;
    }

    private void FixedUpdate()
    {
        Detections();
        JumpBuffer();
        Locomotions();

        inputHandler.jumpFlag = false;
    }

    private void Detections()
    {
        float delta = Time.deltaTime;

        // Coyote time
        if (playerDetection.GroundDetection())
        {
            if (isGrounded == false)
            {
                LandCallBack();
            }
            isGrounded = true;
            coyoteTimeCounter = _coyoteTime;
            playerLocomotion._wallJumpTimeCounter = 0;
        }
        else
        {
            isGrounded = false;
            coyoteTimeCounter -= delta;
        }
    }

    private void Locomotions()
    {
        playerLocomotion.HandleMovement(inputHandler.movement);
        playerLocomotion.HandleFlip(inputHandler.movement);
        playerLocomotion.HandleJump(_jumpBufferTimeCounter > 0, inputHandler.isJumpPerforming);
        playerLocomotion.HandleWallSlide(inputHandler.movement, isGrounded);
        playerLocomotion.HandleWallJump(inputHandler.jumpFlag);
    }

    private void JumpBuffer()
    {
        if (inputHandler.jumpFlag)
        {
            _jumpBufferTimeCounter = _jumpBufferTime;
        }

        _jumpBufferTimeCounter -= Time.deltaTime;
    }

    public bool IsWalled()
    {
        return playerDetection.WallDetection(transform.localScale.x > 0);
    }

    public void JumpCallBack()
    {
        Debug.Log("Jump");
        _jumpBufferTimeCounter = 0;
        TimeshiftManager.instance.ShiftTime();
        Vector3 particlePos = (transform.position - Vector3.up * 0.5f) + Vector3.forward * 10f;
        playerParticleHandler.PlayParticle("Jump", particlePos, transform.rotation, transform);
    }

    public void WallJumpCallBack(float wallJumpDirection)
    {
        Debug.Log("WallJump");
        inputHandler.jumpFlag = false;
        TimeshiftManager.instance.ShiftTime();
        Vector3 particlePos = (transform.position + 0.5f * wallJumpDirection * Vector3.left) + Vector3.forward * 10f;
        playerParticleHandler.PlayParticle("Jump", particlePos,
            Quaternion.Euler(90 * -wallJumpDirection * Vector3.forward), transform);
    }

    private void LandCallBack()
    {
        Debug.Log("Landed");
        Vector3 particlePos = (transform.position - Vector3.up * 0.5f) + Vector3.forward * 10f;
        playerParticleHandler.PlayParticle("Land", particlePos, transform.rotation, transform); 
    }
}
