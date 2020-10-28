using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private GameObject roomPreviewPrefab = null;
    [SerializeField] GameObject test = null;

    private GameObject roomPreviewGO;
    private Camera mainCamera;
    private Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        if(roomPreviewGO != null)
        {
            roomPreviewGO.transform.position = grid.SnapToGrid(GetMousePositionInWorld());
        }
    }

    Vector3 GetMousePositionInWorld()
    {
        return mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
    }

    public void CreateRoom()
    {
        Vector3 roomCenter = grid.SnapToGrid(GetMousePositionInWorld());
        Room roomPrefab = roomPreviewPrefab.GetComponent<Room>();

        if(grid.CanConstructRoom(roomCenter, roomPrefab))
        {
            roomPreviewGO = Instantiate(roomPreviewPrefab, roomCenter, Quaternion.identity);
            Room room = roomPreviewGO.GetComponent<Room>();
            grid.AddRoomInGrid(roomCenter, room);

            foreach (Room nRoom in grid.GetNeighbourRooms(room))
            {
                Instantiate(test, nRoom.CenterPosition, Quaternion.identity);
            }
        }
    }
}
