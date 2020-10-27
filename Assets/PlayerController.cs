using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private float speed = 8f;
    Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        float x = Input.GetAxis("Horizontal");
        rb.velocity += Vector3.right * x * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        
    }
}
