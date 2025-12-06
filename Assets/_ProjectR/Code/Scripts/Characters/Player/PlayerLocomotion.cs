using Unity.Netcode;
using UnityEngine;

public class PlayerLocomotion : NetworkBehaviour
{
    private PlayerDependencies _deps;

    [Header("Movement params")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _acceleration = 20f;

    [Header("Jump params")]
    [SerializeField] private float _jumpHeight = 5f;

    private bool _isJumping = false;
    public bool IsJumping => _isJumping;

    [Header("Ground check")]
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private Vector2 _groundCheckOffset = Vector2.zero;
    public bool IsGrounded { get; private set; } = false;

    [Header("Velocity params")]
    private float _horizontalVel = 0f;
    public float HorizontalVel => _horizontalVel;

    private float _verticalVel = 0f;
    public float VerticalVel => _verticalVel;

    private bool _isFacingRight = true;


    [Header("Mouse params")]
    private Vector3 _mousePosistion;
    private float _screenCenter;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        _deps = GetComponent<PlayerDependencies>();

        // Rigidbosy setup
        _deps.Rigidbody.gravityScale = 3f;
        _deps.Rigidbody.linearDamping = 0f;
    }

    private void Update()
    {
        if (!IsOwner) return;

        CheckMousePosition();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        CheckForGround();
        ApplyHorizontalMovement();

        // Sets the Rigidbody vertical linear velocity
        _verticalVel = _deps.Rigidbody.linearVelocityY;
    }

    private void CheckMousePosition()
    {
        _mousePosistion = Input.mousePosition;
        _screenCenter = Screen.width / 2f;
    }

    public void RotateTowardsMovement()
    {
        if (!IsOwner) return;
        if (_deps.UseItem.IsUsingItem) return;

        if (_isFacingRight && _horizontalVel < 0f)
        {
            _isFacingRight = false;
            transform.Rotate(0f, 180f, 0f);
            return;
        }

        if (!_isFacingRight && _horizontalVel > 0f)
        {
            _isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
            return;
        }
    }

    public void RoatateTowardsAimDirection()
    {
        if (!IsOwner) return;

        if (_isFacingRight && _mousePosistion.x < _screenCenter)
        {
            _isFacingRight = false;
            transform.Rotate(0f, 180f, 0f);
            return;
        }

        if (!_isFacingRight && _mousePosistion.x > _screenCenter)
        {
            _isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
            return;
        }
    }

    private void ApplyHorizontalMovement()
    {
        float targetSpeed = _deps.Input.MovementDirection.x * _moveSpeed;

        _horizontalVel = Mathf.MoveTowards(_horizontalVel, targetSpeed, _acceleration * Time.fixedDeltaTime);

        if (Mathf.Abs(_horizontalVel) < 0.1f && Mathf.Abs(targetSpeed) < 0.1f) _horizontalVel = 0f;

        Vector2 currentVelocity = _deps.Rigidbody.linearVelocity;
        currentVelocity.x = _horizontalVel;
        _deps.Rigidbody.linearVelocity = currentVelocity;
    }
    
    public bool CanJump() => IsGrounded;

    public void Jump()
    {
        if (!IsOwner) return;

        Vector2 velocity = _deps.Rigidbody.linearVelocity;
        float gravity = 9.81f * _deps.Rigidbody.gravityScale;
        float smallJumpHeightAdjust = 0.25f;

        _verticalVel = 0f;
        velocity.y = Mathf.Sqrt(2f * gravity * (_jumpHeight + smallJumpHeightAdjust));
        _deps.Rigidbody.linearVelocity = velocity;
    }

    private void CheckForGround()
    {
        Vector2 origin = (Vector2)transform.position + _groundCheckOffset;
        RaycastHit2D hit = Physics2D.CircleCast(origin, _groundCheckRadius, Vector2.down, 0.01f, _groundLayers);
        IsGrounded = hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Vector2 origin = (Vector2)transform.position + _groundCheckOffset;
        Gizmos.DrawWireSphere(origin, _groundCheckRadius);
    }
}
