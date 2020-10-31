using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveTrapAbstract : MonoBehaviour
{
    private void Update()
    {
        if (TrapTriggered())
        {
            ActivateTrap();
        }
    }

    public abstract void ActivateTrap();
    public abstract bool TrapTriggered();
}