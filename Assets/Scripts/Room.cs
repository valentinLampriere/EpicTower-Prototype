using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public float Width;
    public float Height;

    public bool IdleTrapPlaced = false;

    public Vector3 CenterPosition { get; set; }
    public List<Room> NeigbourRooms { get; set; }

    private GameObject PlacedTrap = null;

    public void PlaceTrap(GameObject trap)
    {
        if (IdleTrapPlaced)
        {
            DeleteTrap();
        }

        Vector3 TrapPosition = transform.position;
        
        IdleTrapPlaced = true;
        TrapPosition.y -= 1.45f; //sorry for hardcoding them, but I've experienced some problems with proper position calculating
        TrapPosition.z += .5f;
        PlacedTrap = Instantiate(trap, TrapPosition, Quaternion.identity);
    }

    public void DeleteTrap()
    {
        Destroy(PlacedTrap);
        IdleTrapPlaced = false;
    }
}
