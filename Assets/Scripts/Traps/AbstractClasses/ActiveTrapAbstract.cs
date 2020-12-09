using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveTrapAbstract : MonoBehaviour
{
    [Range(0f, 100f)]
    public float CustomCooldown = 10f;
    [Range(0f, 100f)]
    public float TimeActive = 5f;
    [Range(0f, 100f)]
    public float Effect = 10f;

    public TriggerButton Trigger;

    public abstract void ActivateTrap();
    public abstract bool TrapTriggered();

    [SerializeField]
    protected float TimeUntilActive = 0f;

    protected virtual void Update()
    {
        if (TrapTriggered())
        {
            ActivateTrap();
        }
    }
}