using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIdleTrap : IdleTrapAbstract
{
    [Range(0f, 100f)]
    public float Damage;

    protected override void DoAction(Enemy enemy)
    {
        enemy.TakeDamage(Damage);
    }
}
