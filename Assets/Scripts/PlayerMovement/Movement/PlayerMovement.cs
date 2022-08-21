using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IDamage
{
    [Header("Settigs Movement")]
    [SerializeField] private PlayerMovementSettings _dataMovement;
    [Space]
    [Header("Camera Settings")]
    [SerializeField] private Transform _cameraJoint;
    [SerializeField] private SwordController _sword;
    
    private float rotatipnVelocity;
   
    private PlayerInput _inputSettings;
    private CharacterController _characterController;
    private Transform _character;
    private Vector2 _move;
    private Vector2 _look;
    
    private float _fallTimeoutDelta;
    private float _jumpTimeoutDelta;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;
    private AnimatorSwitcher _animSwitcher;

    private int HP;

    private bool block;

    private void Awake()
    {
        _animSwitcher = GetComponentInChildren<AnimatorSwitcher>();
        _inputSettings = new PlayerInput();
        HP = _dataMovement.GetHP;
        block = false;
        _inputSettings.Player.Enable();
        _character = transform.Find("Character");
        _characterController = GetComponentInChildren<CharacterController>();
        _inputSettings.Player.Move.canceled += ctx => _move = ctx.ReadValue<Vector2>();
        _inputSettings.Player.Move.performed += ctx => _move = ctx.ReadValue<Vector2>();
        _inputSettings.Player.Look.performed += Looking;
        _inputSettings.Player.Attack.performed += Attack;
        _inputSettings.Player.Block.performed += Block;
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (!_move.Equals(Vector2.zero))
        {
            _animSwitcher.SwitchAnimation("Move");
            int directionAnim;
            if (_move.x > 0)
            {
                directionAnim = 1;
            }
            else
            {
                if (_move.x < 0)
                {
                    directionAnim = -1;
                }
                else
                {
                    directionAnim = 0;
                }
            }
            _animSwitcher.SetCountAnimator("LeftRight", directionAnim);
            Vector3 targetDirection = GetTargetDirection(GetTargetRotation());
            float rotation = Mathf.SmoothDampAngle(_character.eulerAngles.y, 
                _cameraJoint.eulerAngles.y, ref rotatipnVelocity, _dataMovement.RotationSmoothTime);
            _characterController.Move((targetDirection.normalized * _dataMovement.GetSpeed +
                                       new Vector3(_move.x, 0, _move.y)) * Time.deltaTime);
            _character.rotation = Quaternion.Euler(0, rotation,0);
        }
        else
        {
            _animSwitcher.SwitchAnimation("Idle");
        }
    }

    private void Looking(InputAction.CallbackContext ctx)
    {
        _look = ctx.ReadValue<Vector2>();
        Vector3 rotationCamera = _cameraJoint.localRotation.eulerAngles;

        float X = rotationCamera.x + _look.y;
        if (X > 300)
        {
            X -= 360;
        }
        X = Mathf.Clamp(X, _dataMovement.MinXAngle, _dataMovement.MaxXAngle);
        float Y = rotationCamera.y + _look.x;
        _cameraJoint.SetPositionAndRotation(_cameraJoint.position, Quaternion.Euler(new Vector3(X, Y)));
    }

    private Vector3 GetTargetDirection( float targetRotation)
    {
        return Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
    }

    private float GetTargetRotation()
    {
        Vector3 inputDirection = new Vector3(_move.x, 0.0f, _move.y).normalized;
        float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
               _cameraJoint.transform.eulerAngles.y;
        return targetRotation;
    }

    private void Attack(InputAction.CallbackContext ctx)
    {
        _animSwitcher.SwitchAnimation("Attack");
        _sword.Attack = ctx.performed;
    }

    private void Block(InputAction.CallbackContext ctx)
    {
        block = !block;
        _animSwitcher.SwitchAnimation("Block");
    }

    public void GetDamage(int countDamage)
    {
        _animSwitcher.SwitchAnimation("Damage");
        if (!block)
        {
            HP -= countDamage;
            if (HP <= 0)
            {
                LevelBuilder.Instance.RespawnObject(gameObject);
            }
        }
    }

    public void Die()
    {
        _inputSettings.Player.Disable();
        _animSwitcher.SwitchAnimation("Die");
    }

    public void Respawned()
    {
        HP = _dataMovement.GetHP;
        _inputSettings.Player.Enable();
        _animSwitcher.SwitchAnimation("Respawn");
    }
}
