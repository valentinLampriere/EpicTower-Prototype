using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowIdleTrap : IdleTrapAbstract
{
    [Range(0f, 100f)]
    public float Slow;

    protected override void DoAction(Enemy enemy)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().AffectSpeed(Slow / 100f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().ReverseSpeed();
        }
    }
}
