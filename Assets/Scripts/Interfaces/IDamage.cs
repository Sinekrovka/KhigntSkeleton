using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void GetDamage(int countDamage);
    void Respawned();
    void Die();
}
