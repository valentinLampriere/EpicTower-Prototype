using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTrap : AbstractManualTrap
{    
    public Turret TurretObject;

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

    protected override void Update()
    {
        base.Update();
        CheckCooldown();
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
        TurretObject.ChangeDamage(Effect);
    }

    private void Start()
    {
        SetUpTurret();
    }
}
