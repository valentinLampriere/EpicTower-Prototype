using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private int nbCellsWidth = 0;
    [SerializeField] private int nbCellsHeight = 0;
    [SerializeField] private float cellsSize = 1f;

    internal Dictionary<Vector3, Room> DicCells { get; private set; }

    void Awake()
    {
        DicCells = new Dictionary<Vector3, Room>();
        InitGrid();
    }

    void InitGrid()
    {
        Vector3 gridBase = transform.position;
        gridBase = new Vector3(Mathf.FloorToInt(gridBase.x), Mathf.FloorToInt(gridBase.y), 0);
        transform.position = gridBase;

        for (float x = gridBase.x; x < nbCellsWidth; x += cellsSize)
        {
            for (float y = gridBase.y; y < nbCellsHeight; y += cellsSize)
            {
                DicCells.Add(new Vector3((int)x, (int)y, 0), null);
            }
        }
    }

    //bool IsPossibleNeighbour(List<Room> neighbourRooms)
    //{
    //    while(neighbourRooms.Count > 0)
    //    {
    //        Room crtRoom = neighbourRooms[0];
    //        neighbourRooms.RemoveAt(0);

    //        foreach (Room room in neighbourRooms)
    //        {
    //            if (crtRoom.Equals(room))
    //            {
    //                return true;
    //            }
    //        }
    //    }

    //    return false;
    //}

    //bool IsVerticeRoomCell(Vector3 cellPos, Vector3 roomCenter, Room room)
    //{
    //    return ((cellPos.x == (roomCenter.x - room.Width / 2) || cellPos.x == (roomCenter.x + (room.Width / 2) - 1))
    //        && (cellPos.y == (roomCenter.y - room.Height / 2) || cellPos.y == (roomCenter.y + (room.Height / 2) - 1)));
    //}

    public void AddRoomInGrid(Vector3 roomCenter, Room room)
    {
        for (float x = roomCenter.x - (room.Width / 2) ; x < roomCenter.x + (room.Width / 2); x += cellsSize)
        {
            for (float y = roomCenter.y - (room.Height / 2); y < roomCenter.y + (room.Height / 2); y += cellsSize)
            {
                Vector3 cellPos = new Vector3(x, y, 0);

                if(DicCells.ContainsKey(cellPos))
                {
                    room.CenterPosition = roomCenter;
                    DicCells[cellPos] = room;
                }
                else
                {
                    Debug.Log("No key " + cellPos + " in grid");
                }
            }
        }
    }

    public bool IsEmptySpace(Vector3 roomCenter, Room room)
    {
        for (float x = roomCenter.x - (room.Width / 2); x < roomCenter.x + (room.Width / 2); x += cellsSize)
        {
            for (float y = roomCenter.y - (room.Height / 2); y < roomCenter.y + (room.Height / 2); y += cellsSize)
            {
                Vector3 cellPos = new Vector3(x, y, 0);

                if (DicCells.ContainsKey(cellPos) && DicCells[cellPos] != null)
                {
                    Debug.Log("There is already a room at " + roomCenter);
                    return false;
                }
            }
        }

        return true;
    }

    public bool CanConstructRoom(Vector3 roomCenter, Room room)
    {
        if(!IsEmptySpace(roomCenter, room))
        {
            return false;
        }

        // For each edge of the room
        for (float x = roomCenter.x - (room.Width / 2) - 1; x <= roomCenter.x + (room.Width / 2); x += room.Width + 1)
        {
            // Check if there is a non null room in the neighbourhood
            for (float y = roomCenter.y - (room.Height / 2) + 1; y < roomCenter.y + (room.Height / 2) - 1; y += cellsSize)
            {
                Vector3 cellPos = new Vector3(x, y, 0);

                if (DicCells.ContainsKey(cellPos) && DicCells[cellPos] != null)
                {
                    return true;
                }
            }
        }

        for (float y = roomCenter.y - (room.Height / 2) - 1; y <= roomCenter.y + (room.Height / 2); y += room.Height + 1)
        {
            // Check if there is a non null room in the neighbourhood
            for (float x = roomCenter.x - (room.Width / 2) + 1; x < roomCenter.x + (room.Width / 2) - 1; x += cellsSize)
            {
                Vector3 cellPos = new Vector3(x, y, 0);

                if (DicCells.ContainsKey(cellPos) && DicCells[cellPos] != null)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public List<Tuple<Room, bool>> FindNeighbourRooms(Room room)
    {
        List<Tuple<Room, bool>> neighbourRooms = new List<Tuple<Room, bool>>();

        bool ContainsNeighbourRoom(Room _neighRoom, List<Tuple<Room, bool>> _neighbourRooms)
        {
            foreach (Tuple<Room, bool> neighTuple in _neighbourRooms)
            {
                if (neighTuple.Item1.Equals(_neighRoom))
                {
                    return true;
                }
            }
            return false;
        }

        for (float x = room.CenterPosition.x - (room.Width / 2) - 1; x <= room.CenterPosition.x + (room.Width / 2); x += room.Width + 1)
        {
            for (float y = room.CenterPosition.y - (room.Height / 2) + 1; y < room.CenterPosition.y + (room.Height / 2) - 1; y += cellsSize)
            {
                Vector3 cellPos = new Vector3(x, y, 0);

                if (DicCells.ContainsKey(cellPos) && DicCells[cellPos] != null && DicCells[cellPos] != room && !ContainsNeighbourRoom(DicCells[cellPos], neighbourRooms))
                {
                    neighbourRooms.Add(Tuple.Create(DicCells[cellPos], false));
                }
            }
        }

        for (float y = room.CenterPosition.y - (room.Height / 2) - 1; y <= room.CenterPosition.y + (room.Height / 2); y += room.Height + 1)
        {
            for (float x = room.CenterPosition.x - (room.Width / 2) + 1; x < room.CenterPosition.x + (room.Width / 2) - 1; x += cellsSize)
            {
                Vector3 cellPos = new Vector3(x, y, 0);

                if (DicCells.ContainsKey(cellPos) && DicCells[cellPos] != null && DicCells[cellPos] != room && !ContainsNeighbourRoom(DicCells[cellPos], neighbourRooms))
                {
                    neighbourRooms.Add(Tuple.Create(DicCells[cellPos], false));
                }
            }
        }

        return neighbourRooms;
    }

    public Vector3 SnapToGrid(Vector3 position)
    {
        Vector3 snpPos = new Vector3(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), 0);

        if(DicCells.ContainsKey(snpPos))
        {
            return snpPos;
        }
        else
        {
            return Vector3.zero;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (DicCells != null)
        {
            foreach (Vector3 cellPos in DicCells.Keys)
            {
                Gizmos.DrawWireCube(cellPos + new Vector3(cellsSize / 2, cellsSize / 2, 0), Vector3.one * cellsSize);
            }
        }
    }
}
