using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private bool _attack;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamage damage) && _attack )
        {
            damage.GetDamage(Random.Range(0, 20));
            /*Здесь можно издать прикольный звук*/
        }
    }

    public bool Attack
    {
        get => _attack;
        set => _attack = value;
    }
}
