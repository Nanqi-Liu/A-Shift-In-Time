using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerManager _playerManager;

    private float gravityScale;

    [Header("Movement Stats")]
    [SerializeField]
    private float _moveSpeed = 5f;

    [Header("Jump Stats")]
    [SerializeField]
    private float _jumpHeight = 2.5f;
    [SerializeField]
    private float _jumpCutOffGravityMultiplier = 3f;
    private float _maxJumpForce;

    [Header("Wall Jump Stats")]
    [SerializeField]
    private float _wallSlideSpeed = 3f;

    [SerializeField]
    private float _wallJumpDuration = 0.4f;
    public float _wallJumpDirection;
    [SerializeField]
    private float _wallJumpTimeWindow = 0.2f;
    public float _wallJumpTimeCounter;
    [SerializeField]
    private Vector2 _wallJumpForce;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerManager = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        gravityScale = _rb.gravityScale;
        _maxJumpForce = Mathf.Sqrt(2 * Physics2D.gravity.magnitude * _rb.gravityScale * _jumpHeight);
    }

    public void HandleMovement(float movementInput)
    {
        // Movement
        if (!_playerManager.isWallJumping)
        {
            _rb.velocity = new Vector2(_moveSpeed * movementInput, _rb.velocity.y);
        }
    }

    public void HandleFlip(float movementInput)
    {

        if (((movementInput > 0 && !_playerManager.isFacingRight) || (movementInput < 0 && _playerManager.isFacingRight))
            && !_playerManager.isWallJumping)
        {
            Flip();
        }
    }

    public void HandleJump(bool jumpFlag, bool isJumpPerforming)
    {
        if (jumpFlag && _playerManager.coyoteTimeCounter > 0)
        {
            _playerManager.JumpCallBack();
            _playerManager.coyoteTimeCounter = 0;
            _rb.velocity = new Vector2(_rb.velocity.x, _maxJumpForce);
        }

        // Jump cut off when releasing jump key mid jump
        if (_rb.velocity.y > 0.01f)
        {
            if (isJumpPerforming)
            {
                _rb.gravityScale = gravityScale * 1f;
            }
            else
            {
                _rb.gravityScale = gravityScale * _jumpCutOffGravityMultiplier;
            }
        }
        else
        {
            _rb.gravityScale = gravityScale * 1f;
        }
    }

    public void HandleWallSlide(float movementInput, bool isGrounded)
    {
        if(_playerManager.IsWalled() && !isGrounded)
        {
            _playerManager.isWallSliding = true;
            _playerManager.coyoteTimeCounter = 0;
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -_wallSlideSpeed, float.MaxValue));
        }
        else
        {
            _playerManager.isWallSliding = false;
        }
    }

    public void HandleWallJump(bool jumpFlag)
    {
        if (_playerManager.isWallSliding)
        {
            _playerManager.isWallJumping = false;
            _wallJumpDirection = -transform.localScale.x;
            _wallJumpTimeCounter = _wallJumpTimeWindow;

            CancelInvoke(nameof(StopWallJump));
        }
        else
        {
            _wallJumpTimeCounter -= Time.deltaTime;
        }

        if (_wallJumpTimeCounter > 0 && jumpFlag)
        { 
            _playerManager.isWallJumping = true;
            _wallJumpTimeCounter = 0;
            _rb.velocity = new Vector2(_wallJumpDirection * _wallJumpForce.x, _maxJumpForce * _wallJumpForce.y);

            if (transform.localScale.x != _wallJumpDirection)
            {
                Flip();
            }
            _playerManager.WallJumpCallBack(_wallJumpDirection);

            Invoke(nameof(StopWallJump), _wallJumpDuration);
        }
    }

    private void StopWallJump()
    {
        _playerManager.isWallJumping = false;
    }

    private void Flip()
    {
        _playerManager.isFacingRight = !_playerManager.isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}
