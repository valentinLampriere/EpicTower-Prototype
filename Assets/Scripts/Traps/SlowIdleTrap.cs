using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowIdleTrap : IdleTrapAbstract
{
    [Range(0f, 100f)]
    public float Slow;

    protected override void ActionOnEnter(Enemy enemy)
    {
        enemy.AffectSpeed(Slow / 100f);
    }

    protected override void ActionOnExit(Enemy enemy)
    {
        enemy.ReverseSpeed();
    }

    protected override void ActionOnStay(Enemy enemy)
    {

    }
}
