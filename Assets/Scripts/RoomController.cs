using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private GameObject roomPreviewPrefab = null;

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
        Room room = roomPreviewPrefab.GetComponent<Room>();

        if(grid.CanConstructRoom(roomCenter, room))
        {
            roomPreviewGO = Instantiate(roomPreviewPrefab, roomCenter, Quaternion.identity);
            grid.AddRoomInGrid(roomCenter, room);
        }
    }
}
