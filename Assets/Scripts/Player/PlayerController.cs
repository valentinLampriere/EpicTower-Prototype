using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask GalleryMask;
    public LayerMask LadderMask;

    public enum PlayerState
    {
        Walking,
        Climbing
    }

    private PlayerState state = PlayerState.Walking;

    public CameraController cameraController;
    public CanvasController canvasController;
    public RoomController roomController;

    [SerializeField]
    private List<AbstractIdleTrap> IdleTraps = new List<AbstractIdleTrap>();

    private int currentTrap = 0;

    [SerializeField]
    private Room currentRoom = null;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float climbSpeed = 1.5f;

    private float xMovement;
    private float yMovement;
    private Vector3 feetPosition;

    private Rigidbody rb;
    private SphereCollider sphereCollider;

    public PlayerState ChangeState(PlayerState _state)
    {
        if (state == PlayerState.Walking && _state == PlayerState.Climbing)
        {
            rb.useGravity = false;
            sphereCollider.isTrigger = true;
            rb.velocity = Vector3.zero;
        }
        else if (state == PlayerState.Climbing && _state == PlayerState.Walking)
        {
            rb.useGravity = true;
            sphereCollider.isTrigger = false;
        }

        state = _state;
        return state;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        canvasController.ChangeIdleTrap(IdleTraps[currentTrap].GetComponent<Renderer>());
    }

    private void FixedUpdate()
    {
        switch (state)
        {
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

    private void Update()
    {
        if (MGR_Game.Instance.GetPhase() == Phase.Phase2)
        {
            xMovement = Input.GetAxisRaw("Horizontal");
            yMovement = Input.GetAxisRaw("Vertical");
            feetPosition = transform.position - new Vector3(0, transform.localScale.y * 1.25f, 0);
            UpdateState();
            RotateMesh();

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

            cameraController.trPlayer = transform;

            if (Input.GetKeyDown(KeyCode.L))
            {
                if (currentRoom != null)
                {
                    roomController.TryToCreateVerticalLadder(currentRoom, false, transform.position.x);
                }
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                if (currentRoom != null)
                {
                    roomController.TryToCreateVerticalLadder(currentRoom, true, transform.position.x);
                }
            }
        }
    }

    private bool IsOnLadder()
    {
        Collider[] ladderColliders = Physics.OverlapSphere(feetPosition, 0.01f, LadderMask);

        return ladderColliders.Length > 0;
    }

    private bool IsGrounded()
    {
        Collider[] galleryColliders = Physics.OverlapSphere(feetPosition, 0.01f, GalleryMask);

        return galleryColliders.Length > 0;
    }

    private void UpdateState()
    {
        if (IsOnLadder() && yMovement != 0)
        {
            ChangeState(PlayerState.Climbing);
        }
        if (IsGrounded() && xMovement != 0)
        {
            ChangeState(PlayerState.Walking);
        }
        if (!IsOnLadder())
        {
            ChangeState(PlayerState.Walking);
        }
    }

    private void RotateMesh()
    {
        float xVel = rb.velocity.x;
        if (Mathf.Abs(xVel) < 1f)
        {
            return;
        }

        float y = xVel > 0 ? 180f : 0f;

        transform.rotation = Quaternion.Euler(0, y, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            currentRoom = other.GetComponent<Room>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
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