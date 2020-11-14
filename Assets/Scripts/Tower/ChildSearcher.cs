using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildSearcher : MonoBehaviour
{
    public Transform GetObjectWithTag(string tag)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if(child.CompareTag(tag))
            {
                return child;
            }
        }

        return null;
    }
}
