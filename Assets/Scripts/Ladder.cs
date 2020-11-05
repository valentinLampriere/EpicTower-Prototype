using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Ladder : MonoBehaviour {

    public SphereCollider End1 { get; set; }
    public SphereCollider End2 { get; set; }
    void Start() {
        End1 = GetComponents<SphereCollider>()[0];
        End2 = GetComponents<SphereCollider>()[1];
    }
    public Vector3 GetOtherEnd(Collider collider) {
        if (collider == End1) {
            return End2.transform.TransformPoint(End2.center);
        } else {
            return End1.transform.TransformPoint(End1.center);
        }
    }
}
