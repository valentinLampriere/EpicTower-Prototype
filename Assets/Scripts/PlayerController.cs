using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public enum PlayerState {
        Moving,
        Climbing
    }

    PlayerState state = PlayerState.Moving;

    private float speed = 8f;
    Rigidbody rb;
    private GameObject ladder;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public PlayerState ChangeState(PlayerState _state) {

        if(state == PlayerState.Moving && _state == PlayerState.Climbing) {
            
        }

        state = _state;
        return state;
    }

    private void FixedUpdate() {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        switch (state) {
            case PlayerState.Moving:
                rb.velocity += Vector3.right * x * speed * Time.fixedDeltaTime;
                break;
            case PlayerState.Climbing:
                rb.velocity += Vector3.up * y * speed * Time.fixedDeltaTime;
                break;
            default:
                break;
        }
    }

    void Update() {
        if(ladder != null && Input.GetAxis("Vertical") != 0) {
            ChangeState(PlayerState.Climbing);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Ladder") {
            ladder = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Ladder") {
            ladder = null;
        }
    }
    /*private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Ladder") {
            if (Input.GetKeyDown(KeyCode.S) && (other.gameObject.transform.position.y - transform.position.y) < 0) {
                ChangeState(PlayerState.Climbing);
            }
            if (Input.GetKeyDown(KeyCode.Z) && (other.gameObject.transform.position.y - transform.position.y) > 0) {
                ChangeState(PlayerState.Climbing);
            }
        }
    }*/
}
