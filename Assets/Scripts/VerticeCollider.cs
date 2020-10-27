using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticeCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.GetType().IsEquivalentTo(typeof(CapsuleCollider)))
        {
            Debug.Log((CapsuleCollider)other);
        }
    }
}
