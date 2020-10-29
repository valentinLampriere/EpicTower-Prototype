using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public float Width;
    public float Height;

    public Vector3 CenterPosition { get; set; }
    public List<Tuple<Room, bool>> NeighbourRooms { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ContainsNeighbourRoom(Room _neighRoom)
    {
        foreach (Tuple<Room, bool> neighTuple in NeighbourRooms)
        {
            if (neighTuple.Item1.Equals(_neighRoom) && neighTuple.Item2 == true)
            {
                return true;
            }
        }

        return false;
    }
}
