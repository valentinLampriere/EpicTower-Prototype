﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomController : MonoBehaviour
{
    [SerializeField] private GameObject ladderEnemyPrefab = null;
    [SerializeField] private GameObject gatewayEnemyPrefab = null;
    [SerializeField] private GameObject ladderPlayerPrefab = null;
    [SerializeField] private GameObject gatewayPlayerPrefab = null;

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

        GameObject gatewayGO = Instantiate(gatewayEnemyPrefab);
        gatewayGO.transform.position = gatewayPos;
        gatewayGO.transform.localScale = new Vector3(gatewayGO.transform.localScale.x, gatewayLength, gatewayGO.transform.localScale.z);
    }

    void CreateHorizontalLadder(Room downRoom, Room upRoom)
    {
        float ladderAnchorX = downRoom.CenterPosition.x < upRoom.CenterPosition.x ?
            downRoom.CenterPosition.x + (downRoom.Width / 2) - 0.75f : downRoom.CenterPosition.x - (downRoom.Width / 2) + 0.75f;

        Vector3 downAnchor = new Vector3(ladderAnchorX, downRoom.CenterPosition.y - downRoom.Height / 2 + 0.5f, -1);
        Vector3 upAnchor = new Vector3(ladderAnchorX, upRoom.CenterPosition.y - upRoom.Height / 2 + 0.5f, -1);

        Vector3 ladderPos = (downAnchor + upAnchor) / 2;
        float ladderLength = upAnchor.y - downAnchor.y;

        GameObject ladderGO = Instantiate(ladderEnemyPrefab, ladderPos, Quaternion.identity);
        ladderGO.transform.localScale = new Vector3(ladderGO.transform.localScale.x, ladderLength, ladderGO.transform.localScale.z);
        NavMeshLink links = ladderGO.GetComponentInChildren<NavMeshLink>();
        links.startPoint = new Vector3(0, -ladderLength / 2, 0);
        links.endPoint = new Vector3(0, ladderLength / 2, 0);
    }

    void CreateHorizontalGalleryGateway(Room downRoom, Room upRoom)
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

        float gatewayAnchorY = downRoom.CenterPosition.y + upRoom.Height / 2 - 1.5f;
        Vector3 leftAnchor = new Vector3(gatewayLeftX, gatewayAnchorY, 1);
        Vector3 rightAnchor = new Vector3(gatewayRightX, gatewayAnchorY, 1);

        Vector3 gatewayPos = (leftAnchor + rightAnchor) / 2;
        float gatewayLength = rightAnchor.x - leftAnchor.x;

        GameObject gatewayGO = Instantiate(gatewayPlayerPrefab);
        gatewayGO.transform.position = gatewayPos;
        gatewayGO.transform.localScale = new Vector3(gatewayGO.transform.localScale.x, gatewayLength, gatewayGO.transform.localScale.z);
    }

    void CreateHorizontalGalleryLadder(Room downRoom, Room upRoom)
    {
        float ladderAnchorX = downRoom.CenterPosition.x < upRoom.CenterPosition.x ?
            upRoom.CenterPosition.x - upRoom.Width / 2 + 0.75f : upRoom.CenterPosition.x + upRoom.Width / 2 - 0.75f;

        Vector3 downAnchor = new Vector3(ladderAnchorX, downRoom.CenterPosition.y + downRoom.Height / 2 - 1.5f, 1);
        Vector3 upAnchor = new Vector3(ladderAnchorX, upRoom.CenterPosition.y + upRoom.Height / 2 - 1.5f, 1);

        Vector3 ladderPos = (downAnchor + upAnchor) / 2;
        float ladderLength = upAnchor.y - downAnchor.y;


        GameObject ladderGO = Instantiate(ladderPlayerPrefab, ladderPos, Quaternion.identity);
        ladderGO.transform.localScale = new Vector3(ladderGO.transform.localScale.x, ladderLength, ladderGO.transform.localScale.z);
    }

    void RemoveWallFromRoom(Room room, string sideWall)
    {
        Destroy(room.GetComponent<ChildSearcher>().FindChildByName(sideWall).gameObject);
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

                    CreateHorizontalGalleryLadder(room, neighRoom);
                    CreateHorizontalGalleryGateway(room, neighRoom);
                }
                else if (room.CenterPosition.y > neighRoom.CenterPosition.y) // Neighbour on left side
                {
                    CreateHorizontalLadder(neighRoom, room);
                    CreateHorizontalGateway(neighRoom, room);

                    CreateHorizontalGalleryLadder(neighRoom, room);
                    CreateHorizontalGalleryGateway(neighRoom, room);
                }
                else
                {
                    CreateHorizontalGateway(room, neighRoom);
                    CreateHorizontalGalleryGateway(room, neighRoom);
                }

                if(room.CenterPosition.x < neighRoom.CenterPosition.x)
                {
                    RemoveWallFromRoom(room, "RightWall");
                    RemoveWallFromRoom(neighRoom, "LeftWall");
                }
                else if(room.CenterPosition.x > neighRoom.CenterPosition.x)
                {
                    RemoveWallFromRoom(neighRoom, "RightWall");
                    RemoveWallFromRoom(room, "LeftWall");
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
