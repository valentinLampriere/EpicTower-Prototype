using System;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public float Width;
    public float Height;
    public float Thickness;
    public bool IdleTrapPlaced = false;
    public Transform BottomBlock;

    public Vector3 CenterPosition { get; set; }
    public List<Tuple<Room, bool>> NeighbourRooms { get; set; }

    public Room DoorLink { get; set; }

    private GameObject PlacedTrap = null;

    public void PlaceTrap(GameObject trap)
    {
        if (IdleTrapPlaced)
        {
            DeleteTrap();
        }

        Vector3 TrapPosition = transform.position;

        IdleTrapPlaced = true;
        TrapPosition.y -= Height / 2f - BottomBlock.localScale.y - trap.transform.localScale.y / 2f;
        TrapPosition.z += Thickness / 4;
        PlacedTrap = Instantiate(trap, TrapPosition, Quaternion.identity, transform);
    }

    public void DeleteTrap()
    {
        Destroy(PlacedTrap);
        IdleTrapPlaced = false;
    }

    public bool HasHorizontalNeighbourOn(bool left)
    {
        foreach (Tuple<Room, bool> neighTuple in NeighbourRooms)
        {
            Vector3 neighRoomFirstVertice = neighTuple.Item1.CenterPosition - new Vector3(neighTuple.Item1.Width / 2, neighTuple.Item1.Height / 2, 0);
            Vector3 neighRoomLastVertice = neighTuple.Item1.CenterPosition + new Vector3(neighTuple.Item1.Width / 2, neighTuple.Item1.Height / 2, 0);

            if (neighRoomLastVertice.x == CenterPosition.x - (Width / 2) && left)
            {
                return true;
            }
            else if (neighRoomFirstVertice.x == CenterPosition.x + (Width / 2) && !left)
            {
                return true;
            }
        }

        return false;
    }

    public int GetTupleIndexOfRoom(Room room)
    {
        for (int i = 0; i < NeighbourRooms.Count; i++)
        {
            if (NeighbourRooms[i].Item1.Equals(room))
            {
                return i;
            }
        }

        Debug.Log("No index found");
        return -1;
    }
}