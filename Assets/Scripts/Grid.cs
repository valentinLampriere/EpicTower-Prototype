using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private int nbCellsWidth = 0;
    [SerializeField] private int nbCellsHeight = 0;
    [SerializeField] private float cellsSize = 1f;

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

    public Vector3 SnapToGrid(Vector3 position)
    {
        Vector3 snpPos = new Vector3(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), 0);

        if(DicCells.ContainsKey(snpPos))
        {
            return snpPos;
        }
        else
        {
            Debug.Log("No key at position : " + snpPos);
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
