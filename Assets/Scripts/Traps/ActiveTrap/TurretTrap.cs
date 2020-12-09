using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTrap : ActiveTrapAbstract
{
    [Range(0f, 100f)]
    public float Damage = 10f;
    [Range(0f, 100f)]
    public float CustomCooldown = 10f;
    [Range(0f, 100f)]
    public float TimeActive = 5f;
    
    public Room BaseRoom;
    public Turret TurretObject;
    public TriggerButton Trigger;

    [SerializeField]
    private float TimeUntilActive = 0f;

    public override void ActivateTrap()
    {
        TimeUntilActive = CustomCooldown;

        TurretObject.ActivateFor(TimeActive);
    }

    public override bool TrapTriggered()
    {
        if (Trigger.Activated && TimeUntilActive <= 0f)
        {
            return true;
        }

        return false;
    }

    private void CheckCooldown()
    {
        if (TimeUntilActive > 0f)
        {
            TimeUntilActive -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TurretObject.AddTarget(other.GetComponent<Enemy>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TurretObject.DeleteTarget(other.GetComponent<Enemy>());
        }
    }

    private void SetUpTurret()
    {
        TurretObject.Damage = Damage;
        TurretObject.TimeActive = TimeActive;
    }

    private void Start()
    {
        SetUpTurret();
    }

    protected override void Update()
    {
        base.Update();
        CheckCooldown();
    }
}
