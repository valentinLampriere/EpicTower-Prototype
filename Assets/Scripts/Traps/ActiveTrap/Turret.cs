using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform TurretCanon; //just doing it for the esthetic purposes
    public LineRenderer ProjectileRenderer; //feel free to delete these later

    [SerializeField]
    private List<Enemy> Targets;
    [SerializeField]
    private Enemy CurrentTarget;
    
    private float TimeActive = 0f;
    private float Damage = 0f;

    public void ActivateFor(float time)
    {
        TimeActive = time;
    }

    public void AddTarget(Enemy Target)
    {
        Targets.Add(Target);
    }

    public void DeleteTarget(Enemy Target)
    {
        if (Targets.Contains(Target))
        {
            Targets.Remove(Target);
            if(CurrentTarget == Target)
            {
                TryGetTarget();
            }
        }
    }
    
    public int GetEnemiesAmount()
    {
        return Targets.Count;
    }

    public void ChangeDamage(float newDamage)
    {
        Damage = newDamage;
    }

    private void Shoot()
    {
        //Debug.Log("Message: Attacking");

        CurrentTarget.TakeDamage(Damage * Time.deltaTime);
        TurretCanon.LookAt(CurrentTarget.transform);

        ProjectileRenderer.SetPosition(1, CurrentTarget.transform.position);
        ProjectileRenderer.enabled = true;
    }

    private void Update()
    {
        if (TimeActive > 0f)
        {
            TimeActive -= Time.deltaTime;
            if (CurrentTarget)
            {
                if (!CurrentTarget.isActiveAndEnabled)
                {
                    DeleteTarget(CurrentTarget);
                    if (!TryGetTarget())
                    {
                        ResetRotation();
                    }
                }

                Shoot();
            }
            else
            {
                TryGetTarget();
                ResetRotation();
            }
        }
        else
        {
            ResetRotation();
        }
    }

    private void ResetRotation()
    {
        if (TurretCanon.rotation != Quaternion.identity)
        {
            TurretCanon.rotation = Quaternion.identity;
            ProjectileRenderer.enabled = false;
        }
    }

    private bool TryGetTarget()
    {
        if (GetEnemiesAmount() > 0)
        {
            CurrentTarget = Targets[0];

            return true;
        }

        CurrentTarget = null;

        return false;
    }

    private void Start()
    {
        Targets = new List<Enemy>();

        ProjectileRenderer.SetPosition(0, TurretCanon.position);
        ProjectileRenderer.enabled = false;
    }
}
