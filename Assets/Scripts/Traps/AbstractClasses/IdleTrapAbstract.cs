using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IdleTrapAbstract : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            DoAction(other.GetComponent<Enemy>());
        }
    }

    protected abstract void DoAction(Enemy enemy);
}
