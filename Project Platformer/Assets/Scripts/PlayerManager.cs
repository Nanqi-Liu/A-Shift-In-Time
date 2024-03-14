using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    InputHandler inputHandler;
    PlayerLocomotion playerLocomotion;
    PlayerDetection playerDetection;
    PlayerParticleHandler playerParticleHandler;

    public bool isTimeshiftWhenJump = true;

    [Header("Player Flags")]
    public bool isFacingRight = true;
    public bool isGrounded;
    public bool isWallSliding;
    public bool isWallJumping;

    // I don't like how I put these two under PlayerManager
    // TODO: Refactor them into PlayerLocomotion if possible
    [Header("Coyote Time")]
    [SerializeField] private float _coyoteTime = 0.2f;
    public float coyoteTimeCounter;

    [Header("Jump Buffer")]
    [SerializeField] private float _jumpBufferTime = 0.2f;
    private float _jumpBufferTimeCounter;

    private void Awake()
    {
        instance = this;
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

        PlayWallSlidesParticle();

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

    public void PlayWallSlidesParticle()
    {
        if (isWallSliding)
            playerParticleHandler.PlayWallSlideParticle();
        else
            playerParticleHandler.StopWallSlideParticle();
    }

    public void JumpCallBack()
    {
        Debug.Log("Jump");
        // Reset jump buffer
        _jumpBufferTimeCounter = 0;

        // Timeshift when jump
        if (isTimeshiftWhenJump)
        {
            TimeshiftManager.instance.ShiftTime();
        }
        
        // Generate jump particle
        Vector3 particlePos = (transform.position - Vector3.up * 0.5f) + Vector3.forward * 10f;
        playerParticleHandler.PlayParticle("Jump", particlePos, transform.rotation, transform);

        // Play jump sound
        AudioManager.instance.PlaySound("Jump");
    }

    public void WallJumpCallBack(float wallJumpDirection)
    {
        Debug.Log("WallJump");
        // Reset jump flag input
        inputHandler.jumpFlag = false;

        // Timeshift when walljump
        if (isTimeshiftWhenJump)
        {
            TimeshiftManager.instance.ShiftTime();
        }
        
        // Generate walljump particle at the back of the player towards wall jump direction
        Vector3 particlePos = (transform.position + 0.5f * wallJumpDirection * Vector3.left) + Vector3.forward * 10f;
        playerParticleHandler.PlayParticle("Jump", particlePos,
            Quaternion.Euler(90 * -wallJumpDirection * Vector3.forward), transform);

        // Play wall jump sound
        AudioManager.instance.PlaySound("Jump");
    }

    private void LandCallBack()
    {
        Debug.Log("Landed");
        // Rest wall jump flag
        isWallJumping = false;

        // Generate Landing particle
        Vector3 particlePos = (transform.position - Vector3.up * 0.5f) + Vector3.forward * 10f;
        playerParticleHandler.PlayParticle("Land", particlePos, transform.rotation, transform); 
    }
}
