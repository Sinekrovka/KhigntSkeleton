using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamage damage))
        {
            damage.GetDamage(Random.Range(0, 20));
        }
    }
}
