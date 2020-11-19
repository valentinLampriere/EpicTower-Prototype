using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomController : MonoBehaviour
{
    [SerializeField] private GameObject ladderPrefab = null;
    [SerializeField] private GameObject stairsPrefab = null;

    //void CreateLadder(Room room, Room neighRoom)
    //{
    //    Vector3 minRoomAnchor;
    //    Vector3 maxRoomAnchor;
    //    Vector3 minNeighRoomAnchor;
    //    Vector3 maxNeighRoomAnchor;
    //    float yPos;
    //    float dist;

    //    if (room.CenterPosition.y > neighRoom.CenterPosition.y)
    //    {
    //        minRoomAnchor = new Vector3(room.CenterPosition.x - room.Width / 2, room.CenterPosition.y - room.Height / 2);
    //        maxRoomAnchor = new Vector3(room.CenterPosition.x + room.Width / 2, room.CenterPosition.y - room.Height / 2);
    //        minNeighRoomAnchor = new Vector3(neighRoom.CenterPosition.x - neighRoom.Width / 2, neighRoom.CenterPosition.y + neighRoom.Height / 2);
    //        maxNeighRoomAnchor = new Vector3(neighRoom.CenterPosition.x + neighRoom.Width / 2, neighRoom.CenterPosition.y + neighRoom.Height / 2);
    //        yPos = neighRoom.CenterPosition.y + 0.5f;
    //        dist = neighRoom.Height;
    //    }
    //    else
    //    {
    //        minRoomAnchor = new Vector3(room.CenterPosition.x - room.Width / 2, room.CenterPosition.y + room.Height / 2);
    //        maxRoomAnchor = new Vector3(room.CenterPosition.x + room.Width / 2, room.CenterPosition.y + room.Height / 2);
    //        minNeighRoomAnchor = new Vector3(neighRoom.CenterPosition.x - neighRoom.Width / 2, neighRoom.CenterPosition.y - neighRoom.Height / 2);
    //        maxNeighRoomAnchor = new Vector3(neighRoom.CenterPosition.x + neighRoom.Width / 2, neighRoom.CenterPosition.y - neighRoom.Height / 2);
    //        yPos = room.CenterPosition.y + 0.5f;
    //        dist = room.Height;
    //    }

    //    Vector3 ladderPos = new Vector3(
    //        (Mathf.Max(minRoomAnchor.x, minNeighRoomAnchor.x) + Mathf.Min(maxRoomAnchor.x, maxNeighRoomAnchor.x)) / 2,
    //        yPos);

    //    GameObject ladderGO = Instantiate(ladderPrefab, ladderPos, Quaternion.identity);
    //    ladderGO.transform.localScale = new Vector3(ladderGO.transform.localScale.x, dist, ladderGO.transform.localScale.z);
    //    NavMeshLink links = ladderGO.GetComponentInChildren<NavMeshLink>();
    //    links.startPoint = new Vector3(0, -dist / 2, 0);
    //    links.endPoint = new Vector3(0, dist / 2, 0);
    //}

    void CreateHorizontalGateway(Room downRoom, Room upRoom)
    {
        float gatewayLeftX;
        float gatewayRightX;

        if (downRoom.CenterPosition.x < upRoom.CenterPosition.x)
        {
            gatewayLeftX = downRoom.CenterPosition.x + downRoom.Width / 2 - 1;
            gatewayRightX = upRoom.CenterPosition.x - upRoom.Width / 2 + 1;
        }
        else
        {
            gatewayLeftX = upRoom.CenterPosition.x + upRoom.Width / 2 - 1;
            gatewayRightX = downRoom.CenterPosition.x - downRoom.Width / 2 + 1;
        }

        float gatewayAnchorY = upRoom.CenterPosition.y - upRoom.Height / 2 + 0.5f;
        Vector3 leftAnchor = new Vector3(gatewayLeftX, gatewayAnchorY, -1);
        Vector3 rightAnchor = new Vector3(gatewayRightX, gatewayAnchorY, -1);

        Vector3 gatewayPos = (leftAnchor + rightAnchor) / 2;
        float gatewayLength = rightAnchor.x - leftAnchor.x;

        GameObject stairsGO = Instantiate(stairsPrefab);
        stairsGO.transform.position = gatewayPos;
        stairsGO.transform.localScale = new Vector3(stairsGO.transform.localScale.x, gatewayLength, stairsGO.transform.localScale.z);
        NavMeshLink links = stairsGO.GetComponentInChildren<NavMeshLink>();
        links.startPoint = new Vector3(0, -gatewayLength / 2, 0);
        links.endPoint = new Vector3(0, gatewayLength / 2, 0);
        links.UpdateLink();
    }

    void CreateHorizontalLadder(Room downRoom, Room upRoom)
    {
        float ladderAnchorX = downRoom.CenterPosition.x < upRoom.CenterPosition.x ?
            downRoom.CenterPosition.x + downRoom.Width / 2 - 1 : downRoom.CenterPosition.x - downRoom.Width / 2 + 1;

        Vector3 downAnchor = new Vector3(ladderAnchorX, downRoom.CenterPosition.y - downRoom.Height / 2 + 0.5f, -1);
        Vector3 upAnchor = new Vector3(ladderAnchorX, upRoom.CenterPosition.y - upRoom.Height / 2 + 0.5f, -1);

        Vector3 ladderPos = (downAnchor + upAnchor) / 2;
        float ladderLength = upAnchor.y - downAnchor.y; 
    

        GameObject ladderGO = Instantiate(ladderPrefab, ladderPos, Quaternion.identity);
        ladderGO.transform.localScale = new Vector3(ladderGO.transform.localScale.x, ladderLength, ladderGO.transform.localScale.z);
        NavMeshLink links = ladderGO.GetComponentInChildren<NavMeshLink>();
        links.startPoint = new Vector3(0, -ladderLength / 2, 0);
        links.endPoint = new Vector3(0, ladderLength / 2, 0);
    }

    void CreateGalleryStairs(Vector3 roomAnchor, Vector3 neighRoomAnchor)
    {
        Vector3 stairsPos = roomAnchor + neighRoomAnchor;
        stairsPos = new Vector3((stairsPos.x / 2), (stairsPos.y / 2), (stairsPos.z / 2));


        float angle = Mathf.Atan2(neighRoomAnchor.y - roomAnchor.y, neighRoomAnchor.x - roomAnchor.x);
        angle = Mathf.Rad2Deg * angle;

        float dist = (roomAnchor - neighRoomAnchor).magnitude;

        GameObject stairsGO = Instantiate(stairsPrefab, stairsPos, Quaternion.identity);
        stairsGO.transform.localScale = new Vector3(stairsGO.transform.localScale.x, dist, stairsGO.transform.localScale.z);
        stairsGO.transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }

    int NeighbourRelativePosition(Room room, Room neighRoom) // returns 0 if horizontal neighbour, 1 if vertical
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

    public void LinkToNeighbourRooms(Room room)
    {
        for (int i = 0; i < room.NeighbourRooms.Count; i++)
        {
            Room neighRoom = room.NeighbourRooms[i].Item1;
            int relPos = NeighbourRelativePosition(room, neighRoom);

            // If is horizontal neighbour
            if (relPos == 0)
            {
                // Create door and stairs
                if (room.CenterPosition.y < neighRoom.CenterPosition.y) // Neighbour on right side
                {
                    CreateHorizontalLadder(room, neighRoom);
                    CreateHorizontalGateway(room, neighRoom);

                    CreateGalleryStairs(
                        new Vector3(room.CenterPosition.x + (room.Width / 2) - 1, room.CenterPosition.y + (room.Height / 2) - 1.5f, 1),
                        new Vector3(neighRoom.CenterPosition.x - (neighRoom.Width / 2) + 1, neighRoom.CenterPosition.y + (neighRoom.Height / 2) - 1.5f, 1));
                }
                else if (room.CenterPosition.y > neighRoom.CenterPosition.y) // Neighbour on left side
                {
                    CreateHorizontalLadder(neighRoom, room);
                    CreateHorizontalGateway(neighRoom, room);

                    CreateGalleryStairs(
                        new Vector3(room.CenterPosition.x - (room.Width / 2) + 1, room.CenterPosition.y + (room.Height / 2) - 1.5f, 1),
                        new Vector3(neighRoom.CenterPosition.x + (neighRoom.Width / 2) - 1, neighRoom.CenterPosition.y + (neighRoom.Height / 2) - 1.5f, 1));
                }
                else
                {
                    CreateHorizontalGateway(room, neighRoom);
                }


                room.NeighbourRooms[i] = Tuple.Create(neighRoom, true);
                neighRoom.NeighbourRooms[neighRoom.GetTupleIndexOfRoom(room)] = Tuple.Create(room, true);
            }
            // If is vertical neighbour
            else if (relPos == 1)
            {
                //// Find a way from the neighbour to the current room with max distance
                //List<Room> visitedRooms = new List<Room>();
                //List<Room> pathRooms = new List<Room>();
                //bool hasWay = FindWayToVerticalNeighbour(neighRoom, room, neighRoom, visitedRooms, 0, pathRooms);

                //if (!hasWay)
                //{
                //    CreateLadder(room, neighRoom);
                //    room.NeighbourRooms[i] = Tuple.Create(neighRoom, true);
                //    neighRoom.NeighbourRooms[neighRoom.GetTupleIndexOfRoom(room)] = Tuple.Create(room, true);
                //}
            }
        }
    }

    public void AddRoomToHisNeighbours(Room room)
    {
        foreach (Tuple<Room,bool> neighRoomTuple in room.NeighbourRooms)
        {
            neighRoomTuple.Item1.NeighbourRooms.Add(Tuple.Create(room, false));
        }
    }
}
