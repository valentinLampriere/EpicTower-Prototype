using System;
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
    Tuple<Ladder, SphereCollider> climbingLadder;

    Rigidbody rb;
    CapsuleCollider cc;

    void Start() {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
    }

    public PlayerState ChangeState(PlayerState _state) {

        if (state == PlayerState.Walking && _state == PlayerState.Climbing) {
            rb.useGravity = false;
            cc.isTrigger = true;
            rb.velocity = Vector3.zero;
        } else if (state == PlayerState.Climbing && _state == PlayerState.Walking) {
            rb.useGravity = true;
            cc.isTrigger = false;
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
                Vector3 direction = climbingLadder.Item1.GetDirectionEnd(climbingLadder.Item2);
                if (xMovement > 0) {
                    transform.Translate(direction * Time.deltaTime * xMovement);
                } else if (xMovement < 0) {
                    transform.Translate(direction * Time.deltaTime * -xMovement);
                } else if (yMovement > 0) {
                    transform.Translate(direction * Time.deltaTime * yMovement);
                } else if (yMovement < 0) {
                    transform.Translate(direction * Time.deltaTime * -yMovement);
                }

                break;
            default:
                ChangeState(PlayerState.Walking);
                break;
        }
    }

    void Update() {
        xMovement = Input.GetAxisRaw("Horizontal") * moveSpeed;
        yMovement = Input.GetAxisRaw("Vertical") * moveSpeed;
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Ladder")) {
            if (state == PlayerState.Climbing) {
                Vector3 dir;//= climbingLadder.Item1.GetDirectionEnd(climbingLadder.Item1.GetOtherEnd(other));
                dir = climbingLadder.Item1.GetColliderPosition(climbingLadder.Item2);
                if (Vector3.Distance(transform.position, dir) < 1f) {
                    ChangeState(PlayerState.Walking);
                    climbingLadder = null;
                }
            } else {
                Ladder ladder = other.gameObject.GetComponent<Ladder>();
                Vector2 directionLadder = ladder.GetDirectionEnd(ladder.GetOtherEnd(other));


                if ((directionLadder.x > 0.25f && xMovement > 0 || directionLadder.x < -0.25f && xMovement < 0) ||
                    (directionLadder.y > 0.25f && yMovement > 0 || directionLadder.y < -0.25f && yMovement < 0)) {
                    climbingLadder = Tuple.Create(ladder, ladder.GetOtherEnd(other));
                    ChangeState(PlayerState.Climbing);
                }
            }
        }
    }

    /*private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Ladder")) {
            if (state == PlayerState.Climbing) {
                if (climbingLadder.Item2 == other) {
                    ChangeState(PlayerState.Walking);
                    climbingLadder = null;
                }
            }
        }
    }*/
}
