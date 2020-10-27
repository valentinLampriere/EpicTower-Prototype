using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private int nbCellsWidth = 0;
    [SerializeField] private int nbCellsHeight = 0;
    [SerializeField] private float cellsSize = 1f;
    [SerializeField] private GameObject test = null;

    public Dictionary<Vector3, Tuple<Room, bool>> DicCells { get; private set; }

    void Awake()
    {
        DicCells = new Dictionary<Vector3, Tuple<Room, bool>>();
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
                DicCells.Add(new Vector3((int)x, (int)y, 0), new Tuple<Room, bool>(null, false));
            }
        }
    }

    bool IsVerticeRoomCell(Vector3 cellPos, Vector3 roomCenter, Room room)
    {
        return ((cellPos.x == (roomCenter.x - room.Width / 2) || cellPos.x == (roomCenter.x + (room.Width / 2) - 1))
            && (cellPos.y == (roomCenter.y - room.Height / 2) || cellPos.y == (roomCenter.y + (room.Height / 2) - 1)));
    }

    public void AddRoomInGrid(Vector3 roomCenter, Room room)
    {
        for (float x = roomCenter.x - room.Width / 2 ; x < roomCenter.x + room.Width / 2; x += cellsSize)
        {
            for (float y = roomCenter.y - room.Height / 2; y < roomCenter.y + room.Height / 2; y += cellsSize)
            {
                Vector3 cellPos = new Vector3(x, y, 0);

                if(DicCells.ContainsKey(cellPos))
                {
                    DicCells[cellPos] = new Tuple<Room, bool>(room, IsVerticeRoomCell(cellPos, roomCenter, room));
                    if(IsVerticeRoomCell(cellPos, roomCenter, room))
                    {
                        Instantiate(test, cellPos, Quaternion.identity);
                    }
                }
                else
                {
                    Debug.Log("No key " + cellPos + " in grid");
                }
            }
        }
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
