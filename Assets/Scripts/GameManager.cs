using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject towerPrefab = null;
    [SerializeField] NavMeshSurface navMeshSurface = null;

    private Grid grid;
    private RoomController roomController;

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Grid>();
        roomController = GetComponent<RoomController>();

        InitTowerRooms();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (roomController.ActivePreview)
            {
                roomController.CreateRoom();
            }
            else
            {
                roomController.ActivePreview = true;
                roomController.CreateRoomPreview();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            roomController.ActivePreview = false;
            roomController.DestroyRoomPreview();
        }
    }

    void InitTowerRooms()
    {
        Room[] towerRooms = towerPrefab.GetComponentsInChildren<Room>();

        foreach (Room room in towerRooms)
        {
            Vector3 snpPos = grid.SnapToGrid(room.transform.position);
            room.transform.position = snpPos;
            grid.AddRoomInGrid(snpPos, room);
            room.NeighbourRooms = grid.FindNeighbourRooms(room);
            roomController.AddRoomToHisNeighbours(room);
            roomController.LinkToNeighbourRooms(room);
        }
    }

    public void BuildNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }
}
