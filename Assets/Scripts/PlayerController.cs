using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private GameObject FireTrap = null;
    [SerializeField] private GameObject SlowTrap = null;
    [SerializeField] private float speed = 0f;

    public enum PlayerState {
        Moving,
        Climbing
    }

    PlayerState state = PlayerState.Moving;

    private Rigidbody rb;
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
                rb.velocity += Vector3.right * x * speed;
                break;
            case PlayerState.Climbing:
                rb.velocity += Vector3.up * y * speed;
                break;
            default:
                break;
        }
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate(FireTrap, transform.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(SlowTrap, transform.position, Quaternion.identity);
        }

        if (ladder != null && Input.GetAxis("Vertical") != 0) {
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
