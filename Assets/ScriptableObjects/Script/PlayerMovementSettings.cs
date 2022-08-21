using UnityEngine;

[CreateAssetMenu(order = 51, fileName = "PlayerMovementSettings", menuName = "Tools/Player Movement")]
public class PlayerMovementSettings : ScriptableObject
{
    [SerializeField] private float _speedMovement;
    [SerializeField] private float _speedAttack;
    [SerializeField] private int _HP;
    [SerializeField] private float _minXAngle;
    [SerializeField] private float _maxXAngle;
    [SerializeField] private float _rotationSmoothTime;
    [SerializeField] private int _damage;
    public int GetHP => _HP;
    public float GetSpeedAttack => _speedAttack;
    public float GetSpeed => _speedMovement;
    public float MinXAngle => _minXAngle;
    public float MaxXAngle => _maxXAngle;
    public float RotationSmoothTime => _rotationSmoothTime;
    public int Damage => _damage;
}
