using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    public Vector3 Destination { get; set; }

    public NavMeshAgent Agent { get; set; }

    [SerializeField]
    private float Health = 200f;
    [SerializeField]
    private float currentSpeed = 0f; // just for debug purposes
    
    private float oldSpeed = 0f;

    [SerializeField]
    private float valueGold = 10.0f; //how much gold we earn

    [SerializeField]
    private float damageHP = 0.0f;


    protected virtual void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
        if (HasReachedEnd() || !Alive())
        {
            Destroy(gameObject);
            transform.parent.GetComponent<Wave>().CheckEndWave();
        }

        currentSpeed = Agent.speed;
    }

    protected bool Alive()
    {
        bool alive = Health >= 0;
        if(!alive)
        {
            MGR_Game.Instance.EarnGold(valueGold);
        }
        return alive;          
    }

    protected bool HasReachedEnd()
    {
        if (!Agent.pathPending)
        {
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (Agent.hasPath || Agent.velocity.sqrMagnitude < 1f)
                {
                    MGR_Game.Instance.TakeDamage(damageHP);
                    return true;
                }
            }
        }
        return false;
    }

    public void Init(float speed)
    {
        Agent.speed = speed;
        Agent.SetDestination(Destination);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }

    public void ChangeSpeed(float newSpeed)
    {
        oldSpeed = Agent.speed;
        Agent.speed = newSpeed;
    }

    public void AffectSpeed(float incomingMultiplier)
    {
        oldSpeed = Agent.speed;
        Agent.speed *= incomingMultiplier;
    }

    public void RestoreSpeed()
    {
        Agent.speed = oldSpeed;
    }
}