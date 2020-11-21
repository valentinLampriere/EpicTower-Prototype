using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public LayerMask GalleryMask;
    public LayerMask LadderMask;
    public enum PlayerState {
        Walking,
        Climbing
    }

    PlayerState state = PlayerState.Walking;

    public CameraController cameraController;
    public CanvasController canvasController;
    public RoomController roomController;

    [SerializeField]
    private List<IdleTrapAbstract> IdleTraps = new List<IdleTrapAbstract>();
    private int currentTrap = 0;

    [SerializeField]
    private Room currentRoom = null;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float climbSpeed = 1.5f;

    private float xMovement;
    private float yMovement;
    private Vector3 feetPosition;

    Rigidbody rb;
    CapsuleCollider capsCollider;

    void Start() {
        rb = GetComponent<Rigidbody>();
        capsCollider = GetComponent<CapsuleCollider>();
        canvasController.ChangeIdleTrap(IdleTraps[currentTrap].GetComponent<Renderer>());
    }

    private void FixedUpdate() {
        switch (state) {
            case PlayerState.Walking:
                rb.velocity = new Vector3(xMovement * moveSpeed, rb.velocity.y, rb.velocity.z);
                break;
            case PlayerState.Climbing:
                rb.velocity = new Vector3(0, yMovement * climbSpeed, 0);
                break;
            default:
                ChangeState(PlayerState.Walking);
                break;
        }

    }

    void Update() {
        xMovement = Input.GetAxisRaw("Horizontal");
        yMovement = Input.GetAxisRaw("Vertical");
        feetPosition = transform.position - new Vector3(0, transform.localScale.y + 0.05f, 0);
        UpdateState();

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

        if(Input.GetKeyDown(KeyCode.L))
        {
            if(currentRoom != null)
            {
                roomController.TryToCreateVerticalLadder(currentRoom, false, transform.position.x);
            }
        }
        
        if(Input.GetKeyDown(KeyCode.M))
        {
            if (currentRoom != null)
            {
                roomController.TryToCreateVerticalLadder(currentRoom, true, transform.position.x);
            }
        }
    }

    public PlayerState ChangeState(PlayerState _state)
    {

        if (state == PlayerState.Walking && _state == PlayerState.Climbing)
        {
            rb.useGravity = false;
            capsCollider.isTrigger = true;
            rb.velocity = Vector3.zero;
        }
        else if (state == PlayerState.Climbing && _state == PlayerState.Walking)
        {
            rb.useGravity = true;
            capsCollider.isTrigger = false;
        }

        state = _state;
        return state;
    }

    bool IsOnLadder()
    {
        Collider[] ladderColliders = Physics.OverlapSphere(feetPosition, 0.01f, LadderMask);

        return ladderColliders.Length > 0;
    }

    bool IsGrounded()
    {
        Collider[] galleryColliders = Physics.OverlapSphere(feetPosition, 0.01f, GalleryMask);

        return galleryColliders.Length > 0;
    }

    void UpdateState()
    {
        if(IsOnLadder() && yMovement != 0)
        {
            ChangeState(PlayerState.Climbing);
        }
        if(IsGrounded() && xMovement != 0)
        {
            ChangeState(PlayerState.Walking);
        }
        if(!IsOnLadder())
        {
            ChangeState(PlayerState.Walking);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            currentRoom = other.GetComponent<Room>();
        }
    }

    private void OnTriggerExit(Collider other) {
        
        if (other.CompareTag("Room"))
        {
            currentRoom = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(feetPosition, 0.01f);
    }
}
