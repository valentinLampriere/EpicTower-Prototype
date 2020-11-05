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

    public CameraController cameraController;
    public CanvasController canvasController;

    [SerializeField]
    private List<IdleTrapAbstract> IdleTraps = new List<IdleTrapAbstract>();
    private int currentTrap = 0;

    [SerializeField]
    private Room currentRoom = null;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float climbSpeed = 1.5f;

    private float xMovement;
    private float yMovement;

    Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
        canvasController.ChangeIdleTrap(IdleTraps[currentTrap].GetComponent<Renderer>());
    }

    private void FixedUpdate() {
        switch (state) {
            case PlayerState.Walking:
                rb.velocity = new Vector3(xMovement * moveSpeed, rb.velocity.y, rb.velocity.z);
                break;
            case PlayerState.Climbing:
                rb.velocity = new Vector3(xMovement * moveSpeed, yMovement * climbSpeed, rb.velocity.z);
                break;
            default:
                ChangeState(PlayerState.Walking);
                break;
        }
    }

    void Update() {
        xMovement = Input.GetAxisRaw("Horizontal");
        yMovement = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.A))
        {
            currentTrap += 1;
            if (currentTrap > IdleTraps.Count - 1)
            {
                currentTrap = 0;
            }

            canvasController.ChangeIdleTrap(IdleTraps[currentTrap].GetComponent<Renderer>());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentRoom)
            {
                currentRoom.PlaceTrap(IdleTraps[currentTrap].gameObject);
            }
        }

        if (Input.GetMouseButton(1))
        {
            cameraController.ZoomIn(transform.position);
        }

        if (Input.GetMouseButtonUp(1))
        {
            cameraController.ZoomOut();
        }
    }

    private void IgnoreWalls(bool ignore)
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Wall"), ignore);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Floors"), ignore);
    }

    public PlayerState ChangeState(PlayerState _state)
    {

        if (state == PlayerState.Walking && _state == PlayerState.Climbing)
        {
            rb.useGravity = false;
            IgnoreWalls(true);
            rb.velocity = Vector3.zero;
        }
        else if (state == PlayerState.Climbing && _state == PlayerState.Walking)
        {
            rb.useGravity = true;
            IgnoreWalls(false);
        }

        state = _state;
        return state;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            currentRoom = other.GetComponent<Room>();
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Ladder") || other.CompareTag("Stairs")) {

            if (state == PlayerState.Walking)
            {
                ChangeState(PlayerState.Climbing);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        
        if (other.CompareTag("Room"))
        {
            currentRoom = null;
        }
        else if (other.CompareTag("Stairs") || other.CompareTag("Ladder"))
        {
            if(state == PlayerState.Climbing)
            {
                ChangeState(PlayerState.Walking);
            }
        }
    }
}
