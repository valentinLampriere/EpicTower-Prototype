using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomController : MonoBehaviour
{
    [SerializeField] private GameObject roomPreviewPrefab = null;
    [SerializeField] private GameObject ladderPrefab = null;
    [SerializeField] private GameObject doorPrefab = null;
    [SerializeField] private int lengthPathLimit = 0;

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

    void LinkToNeighbourRooms(Room room)
    {
        for(int i = 0; i < room.NeighbourRooms.Count; i++)
        {
            Room neighRoom = room.NeighbourRooms[i].Item1;
            int relPos = NeighbourRelativePosition(room, neighRoom);

            // If is horizontal neighbour
            if (relPos == 0)
            {
                // Create door and stairs
                if(room.CenterPosition.x < neighRoom.CenterPosition.x)
                {
                    CreateLadder(
                        new Vector3(room.CenterPosition.x + (room.Width / 2) - 1, room.CenterPosition.y - (room.Height / 2) + 1, 0),
                        new Vector3(neighRoom.CenterPosition.x - (neighRoom.Width / 2) + 1, neighRoom.CenterPosition.y - (neighRoom.Height / 2) + 1, 0));
                }
                else
                {
                    CreateLadder(
                        new Vector3(room.CenterPosition.x - (room.Width / 2) + 1, room.CenterPosition.y - (room.Height / 2) + 1, 0),
                        new Vector3(neighRoom.CenterPosition.x + (neighRoom.Width / 2) - 1, neighRoom.CenterPosition.y - (neighRoom.Height / 2) + 1, 0));
                }

                room.NeighbourRooms[i] = Tuple.Create(neighRoom, true);
                neighRoom.NeighbourRooms[neighRoom.GetTupleIndexOfRoom(room)] = Tuple.Create(room, true);
            }
            // If is vertical neighbour
            else if(relPos == 1)
            {
                // Find a way from the neighbour to the current room with max distance
                List<Room> visitedRooms = new List<Room>();
                List<Room> pathRooms = new List<Room>();
                bool hasWay = FindWayToVerticalNeighbour(neighRoom, room, neighRoom, visitedRooms, 0, pathRooms);

                if (!hasWay)
                {
                    CreateStairs(room, neighRoom);
                    room.NeighbourRooms[i] = Tuple.Create(neighRoom, true);
                    neighRoom.NeighbourRooms[neighRoom.GetTupleIndexOfRoom(room)] = Tuple.Create(room, true);
                }
            }
        }
    }

    bool FindWayToVerticalNeighbour(Room firstNeighbour, Room room, Room neighRoom, List<Room> visitedRooms, int pathLength, List<Room> pathRooms, bool addToVisited = true)
    {
        if(addToVisited)
        {
            visitedRooms.Add(neighRoom);
            pathLength++;
        }

        if (neighRoom.ContainsNeighbourRoomWithStairs(room) || (neighRoom.ContainsNeighbourRoom(room) && NeighbourRelativePosition(room, neighRoom) == 0))
        {
            Debug.Log(room.CenterPosition + " " + neighRoom.CenterPosition + " " + NeighbourRelativePosition(room, neighRoom));
            return true;
        }
        else
        {
            if(pathLength < lengthPathLimit)
            {
                foreach (Tuple<Room, bool> neighRoomTuple in neighRoom.NeighbourRooms)
                {
                    if (!visitedRooms.Contains(neighRoomTuple.Item1) && neighRoomTuple.Item2 == true)
                    {
                        pathRooms.Add(neighRoom);
                        return FindWayToVerticalNeighbour(firstNeighbour, room, neighRoomTuple.Item1, visitedRooms, pathLength, pathRooms);
                    }
                }
            }

            if(!neighRoom.Equals(firstNeighbour))
            {
                // Get back to the previous room
                pathLength--;
                Room prevRoom = pathRooms[pathRooms.Count - 1];
                pathRooms.RemoveAt(pathRooms.Count - 1);

                return FindWayToVerticalNeighbour(firstNeighbour, room, prevRoom, visitedRooms, pathLength, pathRooms, false);
            }
        }

        return false;
    }

    void CreateStairs(Room room, Room neighRoom)
    {
        Vector3 minRoomAnchor;
        Vector3 maxRoomAnchor;
        Vector3 minNeighRoomAnchor;
        Vector3 maxNeighRoomAnchor;
        float yPos;
        float dist;

        if (room.CenterPosition.y > neighRoom.CenterPosition.y)
        {
            minRoomAnchor = new Vector3(room.CenterPosition.x - room.Width / 2, room.CenterPosition.y - room.Height / 2);
            maxRoomAnchor = new Vector3(room.CenterPosition.x + room.Width / 2, room.CenterPosition.y - room.Height / 2);
            minNeighRoomAnchor = new Vector3(neighRoom.CenterPosition.x - neighRoom.Width / 2, neighRoom.CenterPosition.y + neighRoom.Height / 2);
            maxNeighRoomAnchor = new Vector3(neighRoom.CenterPosition.x + neighRoom.Width / 2, neighRoom.CenterPosition.y + neighRoom.Height / 2);
            yPos = neighRoom.CenterPosition.y + 0.5f;
            dist = neighRoom.Height;
        }
        else
        {
            minRoomAnchor = new Vector3(room.CenterPosition.x - room.Width / 2, room.CenterPosition.y + room.Height / 2);
            maxRoomAnchor = new Vector3(room.CenterPosition.x + room.Width / 2, room.CenterPosition.y + room.Height / 2);
            minNeighRoomAnchor = new Vector3(neighRoom.CenterPosition.x - neighRoom.Width / 2, neighRoom.CenterPosition.y - neighRoom.Height / 2);
            maxNeighRoomAnchor = new Vector3(neighRoom.CenterPosition.x + neighRoom.Width / 2, neighRoom.CenterPosition.y - neighRoom.Height / 2);
            yPos = room.CenterPosition.y + 0.5f;
            dist = room.Height;
        }

        Vector3 stairsPos = new Vector3(
            (Mathf.Max(minRoomAnchor.x, minNeighRoomAnchor.x) + Mathf.Min(maxRoomAnchor.x, maxNeighRoomAnchor.x)) / 2,
            yPos);

        GameObject stairsGO = Instantiate(ladderPrefab, stairsPos, Quaternion.identity);
        stairsGO.transform.localScale = new Vector3(stairsGO.transform.localScale.x, dist, stairsGO.transform.localScale.z);
        NavMeshLink links = stairsGO.GetComponentInChildren<NavMeshLink>();
        links.startPoint = new Vector3(0, -dist / 2, 0);
        links.endPoint = new Vector3(0, dist / 2, 0);
    }

    void CreateLadder(Vector3 roomAnchor, Vector3 neighRoomAnchor)
    {
        Vector3 ladderPos = roomAnchor + neighRoomAnchor;
        float xOffset = roomAnchor.y > neighRoomAnchor.y ? -0.5f : 0.5f;
        xOffset = roomAnchor.x < neighRoomAnchor.x ? -xOffset : xOffset;

        ladderPos = new Vector3((ladderPos.x / 2) + xOffset, (ladderPos.y / 2) - 0.5f, 0);


        float angle = Mathf.Atan2(neighRoomAnchor.y - roomAnchor.y, neighRoomAnchor.x - roomAnchor.x);
        angle = Mathf.Rad2Deg * angle;

        float dist = (roomAnchor - neighRoomAnchor).magnitude;

        GameObject ladderGO = Instantiate(ladderPrefab, ladderPos, Quaternion.identity);
        ladderGO.transform.localScale = new Vector3(ladderGO.transform.localScale.x, dist, ladderGO.transform.localScale.z);
        NavMeshLink links = ladderGO.GetComponentInChildren<NavMeshLink>();
        links.startPoint = new Vector3(0, -dist / 2, 0);
        links.endPoint = new Vector3(0, dist / 2, 0);
        ladderGO.transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }

    // returns 0 if horizontal neighbour, 1 if vertical
    int NeighbourRelativePosition(Room room, Room neighRoom)
    {
        Vector3 neighRoomFirstVertice = neighRoom.CenterPosition - new Vector3(neighRoom.Width / 2, neighRoom.Height / 2, 0);
        Vector3 neighRoomLastVertice = neighRoom.CenterPosition + new Vector3(neighRoom.Width / 2, neighRoom.Height / 2, 0);

        if (neighRoomFirstVertice.x == room.CenterPosition.x + (room.Width / 2) || neighRoomLastVertice.x == room.CenterPosition.x - (room.Width / 2))
        {
            return 0;
        }
        else if(neighRoomFirstVertice.y == room.CenterPosition.y + (room.Height / 2) || neighRoomLastVertice.y == room.CenterPosition.y - (room.Height / 2))
        {
            return 1;
        }

        Debug.Log(room + " " + neighRoom + " PROBLEM");
        return -1;
    }

    void AddRoomToHisNeighbours(Room room)
    {
        foreach (Tuple<Room,bool> neighRoomTuple in room.NeighbourRooms)
        {
            neighRoomTuple.Item1.NeighbourRooms.Add(Tuple.Create(room, false));
        }
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
            room.NeighbourRooms = grid.FindNeighbourRooms(room);
            AddRoomToHisNeighbours(room);
            LinkToNeighbourRooms(room);
        }
    }
}
