using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostAutomaticRoom : AbstractAutomaticRoom
{
    [Range(0f, 100f)]
    public float Slow;

    protected override void ActionOnEnter(Enemy enemy)
    {
        enemy.AffectSpeed(Slow / 100f);
    }

    protected override void ActionOnExit(Enemy enemy)
    {
        enemy.RestoreSpeed();
    }

    protected override void ActionOnStay(Enemy enemy)
    {
        //OnTriggerStay
    }
}
