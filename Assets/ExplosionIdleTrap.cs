using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionIdleTrap : IdleTrapAbstract
{
    [SerializeField]
    private int EnemiesRequirement = -1;
    
    private List<Enemy> EnemiesAOE;
    
    [Range(0, 100f), SerializeField]
    private float BaseDamage = 0f;

    [Range(0, 100f), SerializeField]
    private float DamageCoefficient = 0f;

    [Range(0, 3f), SerializeField]
    private float PushForce = 0f;
    private bool BlownUp = false;

    private void Start()
    {
        EnemiesAOE = new List<Enemy>();
    }

    protected override void ActionOnEnter(Enemy enemy)
    {
        EnemiesAOE.Add(enemy);

        if (EnemiesAOE.Count == EnemiesRequirement && !BlownUp)
        {
            foreach (Enemy EnemyToBlowUp in EnemiesAOE)
            {
                float Damage = BaseDamage - Vector3.Distance(transform.position, enemy.transform.position) / DamageCoefficient;
                Vector3 PushDirection = (enemy.transform.position - transform.position).normalized;

                EnemyToBlowUp.TakeDamage(Damage);
                EnemyToBlowUp.transform.position += PushDirection * PushForce;
            }

            BlownUp = true;
        }
    }

    protected override void ActionOnStay(Enemy enemy)
    {

    }

    protected override void ActionOnExit(Enemy enemy)
    {
        EnemiesAOE.Remove(enemy);
    }
}
