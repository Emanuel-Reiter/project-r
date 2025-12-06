using Unity.Netcode;
using UnityEngine;

public class PlayerLocomotion : NetworkBehaviour
{
    private PlayerDependencies _deps;

    [Header("Movement params")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _acceleration = 20f;

    [Header("Gravity params")]
    [SerializeField] private float _gravity = -12f;
    [SerializeField] private float _maxVerticalVel = -50f;

    [Header("Jump params")]
    [SerializeField] private float _jumpHeight = 4f;
    public bool IsJumping { get; private set; } = false;

    [Header("Ground check")]
    [SerializeField] private LayerMask _groundLayers;
    public bool IsGrounded { get; private set; } = false;

    private float _horizontalVel = 0f;
    public float HorizontalVel => _horizontalVel;

    private float _verticalVel = 0f;
    public float VerticalVel => _verticalVel;

    public float MoveThreshold { get; private set; } = 0.05f;

    private bool _isFacingRight = true;

    [Header("Mouse params")]
    private Vector3 _mousePosistion;
    private float _screenCenter;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        _deps = GetComponent<PlayerDependencies>();
    }

    private void Update()
    {
        if (!IsOwner) return;

        CheckMousePosition();
        CheckForGround();
    }

    private void CheckMousePosition()
    {
        _mousePosistion = Input.mousePosition;
        _screenCenter = Screen.width / 2f;
    }

    public void Move()
    {
        if (!IsOwner) return;

        Vector2 moveVector = new Vector2(_horizontalVel, _verticalVel);

        _deps.Rigidbody.linearVelocity = moveVector;
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

    public void CalculateHorizontalVel()
    {
        _horizontalVel = Mathf.MoveTowards(_horizontalVel, _moveSpeed * _deps.Input.MovementDirection.x, _acceleration * Time.deltaTime);
    }

    public void CalculateVerticalVel()
    {
        if (IsJumping) return;

        if (IsGrounded)
        {
            _verticalVel = _gravity;
        }
        else
        {
            _verticalVel = Mathf.MoveTowards(_verticalVel, _maxVerticalVel, _gravity * Time.deltaTime);
        }
    }

    public void Jump()
    {
        if (!IsOwner || !IsGrounded || IsJumping) return;

        IsJumping = true;

        Debug.Log("Jumped!");

        _verticalVel = -2.0f * _gravity * _jumpHeight;

        Invoke("HandleJump", 0.02f);
    }

    private void HandleJump()
    {
        IsJumping = false;
    }

    private void CheckForGround()
    {
        Vector2 origin = transform.position;
        float radius = 0.333f;

        IsGrounded = Physics2D.OverlapCircle(origin, radius, _groundLayers);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Vector2 origin = transform.position;
        float radius = 0.333f;
        Gizmos.DrawWireSphere(origin, radius);
    }
}
