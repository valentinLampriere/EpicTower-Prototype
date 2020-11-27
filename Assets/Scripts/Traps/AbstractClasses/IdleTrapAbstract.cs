using UnityEngine;

public abstract class IdleTrapAbstract : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            ActionOnEnter(other.GetComponent<Enemy>());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            ActionOnStay(other.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            ActionOnExit(other.GetComponent<Enemy>());
        }
    }

    protected abstract void ActionOnEnter(Enemy enemy);

    protected abstract void ActionOnStay(Enemy enemy);

    protected abstract void ActionOnExit(Enemy enemy);
}