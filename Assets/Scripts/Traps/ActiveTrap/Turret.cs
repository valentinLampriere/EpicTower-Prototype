using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform TurretCanon; //just doing it for esthetic purposes, delete later

    public float TimeActive = 0f;
    public float Damage = 0f;
    public LineRenderer ProjectileRenderer;

    [SerializeField]
    private List<Enemy> Targets;
    [SerializeField]
    private Enemy CurrentTarget;

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
                if(!TryGetTarget())
                {
                    CurrentTarget = null;
                }
            }
        }
    }
    
    public int GetEnemiesAmount()
    {
        return Targets.Count;
    }

    private void Shoot()
    {
        Debug.Log("Message: Attacking");

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
                        ProjectileRenderer.enabled = false;
                        return;
                    }
                }

                Shoot();
            }
            else
            {
                TryGetTarget();
            }
        }
        else
        {
            if (TurretCanon.rotation != Quaternion.identity)
            {
                TurretCanon.rotation = Quaternion.identity;
                ProjectileRenderer.enabled = false;
            }
        }
    }

    private bool TryGetTarget()
    {
        if (GetEnemiesAmount() > 0)
        {
            CurrentTarget = Targets[0];

            return true;
        }

        return false;
    }

    private void Start()
    {
        ProjectileRenderer.SetPosition(0, TurretCanon.position);
        ProjectileRenderer.enabled = false;
        Targets = new List<Enemy>();
    }
}
