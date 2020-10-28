using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public enum PlayerState {
        Walking,
        Climbing
    }

    PlayerState state = PlayerState.Walking;

    private float speed = 8f;
    Rigidbody rb;
    private GameObject ladder;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public PlayerState ChangeState(PlayerState _state) {

        if (state == PlayerState.Walking && _state == PlayerState.Climbing) {
            rb.isKinematic = true;
        } else if (state == PlayerState.Climbing && _state == PlayerState.Walking) {
            rb.isKinematic = false;
        }

        state = _state;
        return state;
    }

    private void FixedUpdate() {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        switch (state) {
            case PlayerState.Walking:
                rb.velocity += Vector3.right * x * speed * Time.fixedDeltaTime;
                break;
            case PlayerState.Climbing:
                rb.position += Vector3.up * y * speed * Time.fixedDeltaTime;
                break;
            default:
                break;
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.S) && (ladder.transform.position.y - transform.position.y) > 0) {
            ChangeState(PlayerState.Climbing);
        }
        if (Input.GetKeyDown(KeyCode.Z) && (ladder.transform.position.y - transform.position.y) < 0) {
            ChangeState(PlayerState.Climbing);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ladder")) {
            ladder = other.gameObject;
            ChangeState(PlayerState.Walking);
            Debug.Log(state);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Ladder")) {
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
