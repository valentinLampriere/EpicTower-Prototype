using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public float Width;
    public float Height;
    public float Thickness;
    public bool IdleTrapPlaced = false;

    public Vector3 CenterPosition { get; set; }
    public List<Tuple<Room, bool>> NeighbourRooms { get; set; }

    private GameObject PlacedTrap = null;

    public void PlaceTrap(GameObject trap)
    {
        if (IdleTrapPlaced)
        {
            DeleteTrap();
        }

        Vector3 TrapPosition = transform.position;
        
        IdleTrapPlaced = true;
        TrapPosition.y -= Height / 2f;
        TrapPosition.y += 0.5f + trap.transform.localScale.y / 2f;
        TrapPosition.z += Thickness / 4;
        PlacedTrap = Instantiate(trap, TrapPosition, Quaternion.identity, transform);
    }

    public void DeleteTrap()
    {
        Destroy(PlacedTrap);
        IdleTrapPlaced = false;
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
