using Unity.Netcode;
using UnityEngine;

public class PlayerLocomotion : NetworkBehaviour
{
    private PlayerDependencies _deps;

    [Header("Movement params")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _acceleration = 40f;

    private Vector2 _currentVelocity = Vector2.zero; 
    public float VelocityMagnitude => _currentVelocity.magnitude;
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
        CheckMousePosition();
    }

    private void CheckMousePosition()
    {
        _mousePosistion =  Input.mousePosition;
        _screenCenter = Screen.width / 2f;
    }

    public void Move()
    {
        if (!IsOwner) return;

        Accelerate(_deps.Input.MovementDirection * _moveSpeed);
        _deps.Rigidbody.linearVelocity = _currentVelocity;
    }

    public void RotateTowardsMovement()
    {
        if (!IsOwner) return;
        if (_deps.UseItem.IsUsingItem) return;

        if (_isFacingRight && _currentVelocity.x < 0f) 
        {
            _isFacingRight = false;
            transform.Rotate(0f, 180f, 0f);
            return;
        }

        if (!_isFacingRight && _currentVelocity.x > 0f)
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

    private void Accelerate(Vector2 targetVelocity)
    {
        _currentVelocity = Vector2.MoveTowards(_currentVelocity, targetVelocity, _acceleration * Time.deltaTime);
    }

    public void Decelerate()
    {
        Accelerate(Vector2.zero);
    }
}
