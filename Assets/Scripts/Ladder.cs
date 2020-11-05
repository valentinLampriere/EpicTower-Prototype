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
    public SphereCollider GetOtherEnd(Collider collider) {
        return (collider == End1) ? End2 : End1;
    }
    public Vector3 GetOtherEndPosition(Collider collider) {
        return GetOtherEnd(collider).transform.TransformPoint(GetOtherEnd(collider).center);
    }

    public Vector3 GetColliderPosition(Collider collider) {
        SphereCollider scollider = GetOtherEnd(GetOtherEnd(collider));
        return scollider.transform.TransformPoint(scollider.center);
    }

    public Vector2 GetDirectionEnd(Collider collider) {
        return (GetColliderPosition(collider) - GetOtherEndPosition(collider)).normalized;
    }
}
