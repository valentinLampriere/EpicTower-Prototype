using UnityEngine;

public class ChildSearcher : MonoBehaviour
{
    public Transform FindChildByName(string name)
    {
        return transform.Find(name);
    }
}