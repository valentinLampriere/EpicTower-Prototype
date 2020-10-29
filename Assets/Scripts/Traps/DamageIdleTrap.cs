using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIdleTrap : IdleTrapAbstract
{
    [Range(0f, 100f)]
    public float Damage;

    protected override void ActionOnEnter(Enemy enemy)
    {
        //OnTriggerEnter
    }

    protected override void ActionOnExit(Enemy enemy)
    {
        //OnTriggerExit
    }

    protected override void ActionOnStay(Enemy enemy)
    {
        enemy.TakeDamage(Damage);
    }
}
