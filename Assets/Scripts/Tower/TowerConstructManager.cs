using UnityEngine;
using UnityEngine.AI;

public class TowerConstructManager : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab = null;
    [SerializeField] private GameObject roomPreviewPrefab = null;
    [SerializeField] private GameObject roomObjectPrefab = null;
    [SerializeField] private NavMeshSurface navMeshSurface = null;
    [SerializeField] private GameObject[] roomsObjectsPrefabs = null;
    [SerializeField] private GameObject[] roomsPreviewPrefabs = null;

    public int currentRoom;
    public int costRoom;

    private Camera mainCamera;
    private Grid grid;
    private GameObject roomGO;
    private GameObject roomPreviewGO;
    private RoomController roomController;

    private bool activePreview;

    // Start is called before the first frame update
    private void Start()
    {
        if (roomsObjectsPrefabs.Length > 0)
            currentRoom = 0;
        else
            Debug.LogError("Aucune Salle !!!");
        mainCamera = Camera.main;
        grid = GetComponent<Grid>();
        roomController = GetComponent<RoomController>();

        InitTowerRooms();
    }

    // Update is called once per frame
    private void Update()
    {
        if (MGR_Game.Instance.GetPhase() == Phase.Phase1)
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
        else
        {
            activePreview = false;
            DestroyRoomPreview();
        }
    }

    private Vector3 GetMousePositionInWorld()
    {
        return mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
    }

    private void InitTowerRooms()
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
            if (MGR_Game.Instance.Buy(costRoom))
            {
                roomGO = Instantiate(roomObjectPrefab, roomCenter, Quaternion.identity);
                Room room = roomGO.GetComponent<Room>();
                SetupRoom(roomCenter, room);
            }
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

    public void SetCurrentRoom(int iRoom)
    {
        currentRoom = Mathf.Clamp(iRoom, 0, roomsObjectsPrefabs.Length - 1);
        roomObjectPrefab = roomsObjectsPrefabs[currentRoom];
        roomPreviewPrefab = roomsPreviewPrefabs[currentRoom];
    }
}