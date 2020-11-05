using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public enum PlayerState {
        Walking,
        Climbing
    }

    PlayerState state = PlayerState.Walking;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float climbSpeed = 1.5f;

    private float xMovement;
    private float yMovement;
    private Vector3 velocity = Vector3.zero;
    private Vector3 climbDirection;

    Rigidbody rb;
    CapsuleCollider cc;

    void Start() {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
    }

    public PlayerState ChangeState(PlayerState _state) {

        if (state == PlayerState.Walking && _state == PlayerState.Climbing) {
        } else if (state == PlayerState.Climbing && _state == PlayerState.Walking) {
        }

        state = _state;
        return state;
    }

    private void FixedUpdate() {
        switch (state) {
            case PlayerState.Walking:
                Vector3 targetVelocity = new Vector3(xMovement, rb.velocity.y);
                rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.3f);
                break;
            case PlayerState.Climbing:
                rb.MovePosition(climbDirection);
                break;
            default:
                ChangeState(PlayerState.Walking);
                break;
        }
    }

    void Update() {
        xMovement = Input.GetAxisRaw("Horizontal") * moveSpeed;
        yMovement = Input.GetAxisRaw("Vertical") * climbSpeed;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ladder")) {
            if (state != PlayerState.Climbing) {
                ChangeState(PlayerState.Climbing);
                climbDirection = other.gameObject.GetComponent<Ladder>().GetOtherEnd(other);
            } else {
                ChangeState(PlayerState.Walking);
            }
        }
    }
}
