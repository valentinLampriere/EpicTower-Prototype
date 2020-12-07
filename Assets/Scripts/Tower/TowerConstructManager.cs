using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TowerConstructManager : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab = null;
    [SerializeField] private GameObject roomPreviewPrefab = null;
    [SerializeField] private GameObject roomObjectPrefab = null;
    [SerializeField] private NavMeshSurface navMeshSurface = null;

    private Camera mainCamera;
    private Grid grid;
    private GameObject roomGO;
    private GameObject roomPreviewGO;
    private RoomController roomController;

    private bool activePreview;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        grid = GetComponent<Grid>();
        roomController = GetComponent<RoomController>();

        InitTowerRooms();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (activePreview)
            {
                CreateRoom();
            }
            else
            {
                activePreview = true;
                CreateRoomPreview();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            activePreview = false;
            DestroyRoomPreview();
        }

        if (roomPreviewGO != null && activePreview)
        {
            roomPreviewGO.transform.position = grid.SnapToGrid(GetMousePositionInWorld());
        }
    }
    private Vector3 GetMousePositionInWorld()
    {
        return mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
    }

    void InitTowerRooms()
    {
        Room[] towerRooms = towerPrefab.GetComponentsInChildren<Room>();

        foreach (Room room in towerRooms)
        {
            Vector3 snpPos = grid.SnapToGrid(room.transform.position);
            room.transform.position = snpPos;
            SetupRoom(snpPos, room);
        }
    }

    private void CreateRoomPreview()
    {
        roomPreviewGO = Instantiate(roomPreviewPrefab, grid.SnapToGrid(GetMousePositionInWorld()), Quaternion.identity);
    }

    private void DestroyRoomPreview()
    {
        Destroy(roomPreviewGO);
    }

    private void CreateRoom()
    {
        Vector3 roomCenter = grid.SnapToGrid(GetMousePositionInWorld());
        Room roomPrefab = roomObjectPrefab.GetComponent<Room>();

        if (grid.CanConstructRoom(roomCenter, roomPrefab))
        {
            roomGO = Instantiate(roomObjectPrefab, roomCenter, Quaternion.identity);
            Room room = roomGO.GetComponent<Room>();
            SetupRoom(roomCenter, room);
        }
    }

    private void SetupRoom(Vector3 roomPos, Room room)
    {
        grid.AddRoomInGrid(roomPos, room);
        room.NeighbourRooms = grid.FindNeighbourRooms(room);
        roomController.AddRoomToHisNeighbours(room);
        roomController.LinkToNeighbourRooms(room);
    }

    public void BuildNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }
}
