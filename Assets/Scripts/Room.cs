using System.Collections;
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
        TrapPosition.y -= Height / 2f - BottomBlock.localScale.y - trap.transform.localScale.y / 2f;
        TrapPosition.z += Thickness / 4;
        PlacedTrap = Instantiate(trap, TrapPosition, Quaternion.identity, transform);
    }

    public void DeleteTrap()
    {
        Destroy(PlacedTrap);
        IdleTrapPlaced = false;
    }
}
