using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public CameraController cameraController;
    public CanvasController canvasController;

    [SerializeField]
    private List<IdleTrapAbstract> IdleTraps = new List<IdleTrapAbstract>();
    private int currentTrap = 0;

    [SerializeField]
    private float speed = 0f;

    [SerializeField]
    private Room currentRoom = null;

    public enum PlayerState {
        Moving,
        Climbing
    }

    PlayerState state = PlayerState.Moving;

    private Rigidbody rb;
    private GameObject ladder;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        canvasController.ChangeIdleTrap(IdleTraps[currentTrap].GetComponent<Renderer>());
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

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentRoom)
            {
                currentTrap += 1;
                if (currentTrap > IdleTraps.Count - 1)
                {
                    currentTrap = 0;
                }
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

        if (Input.GetMouseButtonDown(1))
        {
            cameraController.ZoomIn(transform.position);
        }

        if (Input.GetMouseButtonUp(1))
        {
            cameraController.ZoomOut();
        }

        if (ladder != null && Input.GetAxis("Vertical") != 0) {
            ChangeState(PlayerState.Climbing);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            ladder = other.gameObject;
        }
        else if (other.CompareTag("Room"))
        {
            currentRoom = other.GetComponent<Room>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Ladder"))
        {
            ladder = null;
        }
        else if(other.CompareTag("Room"))
        {
            currentRoom = null;
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
