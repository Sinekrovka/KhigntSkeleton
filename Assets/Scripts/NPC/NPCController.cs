using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour, IDamage
{
    [SerializeField] private SwordController _sword;
    [SerializeField] private int HP;
    private Transform _player;
    private NavMeshAgent _navMeshAgent;
    private AnimatorSwitcher _animSwitcher;
    private bool _block;
    private bool _move;
    private float _attackAnimation = 1f;
    private bool _signalAttack;
    private int _startHP;

    private void Awake()
    {
        _startHP = HP;
        _block = false;
        _move = true;
        _animSwitcher = GetComponentInChildren<AnimatorSwitcher>();
        _player = FindObjectOfType<PlayerMovement>().transform;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.SetDestination(_player.position);
    }

    private void Update()
    {
        if (Vector3.Distance(_player.position, transform.position) <= _navMeshAgent.stoppingDistance)
        {
            if (!_move)
            {
                _animSwitcher.SwitchAnimation("Idle");
                StartCoroutine(WaitAnim());
                _sword.Attack = true;
                _move = true;
            }
            
        }
        else
        {
            _navMeshAgent.SetDestination(_player.position);
            if (_move)
            {
                _move = false;
                _sword.Attack = false;
                StopAllCoroutines();
                _animSwitcher.SwitchAnimation("Attack");
                _animSwitcher.SwitchAnimation("Move");
            }
        }
    }

    public void GetDamage(int countDamage)
    {
        _animSwitcher.SwitchAnimation("Damage");
        if (!_block)
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
        _navMeshAgent.enabled = false;
        _animSwitcher.SwitchAnimation("Die");
    }

    public void Respawned()
    {
        HP = _startHP;
        _navMeshAgent.enabled = true;
        _animSwitcher.SwitchAnimation("Respawn");
    }

    private IEnumerator WaitAnim()
    {
        _animSwitcher.SwitchAnimation("Attack");
        yield return new WaitForSeconds(_attackAnimation);
        StartCoroutine(WaitAnim());
    }
}
