using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTestPlayerController : MonoBehaviour
{
    public float Speed;

    public GameObject FireTrap;
    public GameObject SlowTrap;


    private void Update()
    {
        /*if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.up * Speed * Time.deltaTime;
            Vector3 newPos = transform.position;
            newPos.y += 1;
            transform.position = newPos;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += -transform.right * Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += -transform.up * Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Speed * Time.deltaTime;
        }*/

        if (Input.GetKeyDown(KeyCode.W))
        {
            Vector3 newPos = transform.position;
            newPos.y += 4;
            transform.position = newPos;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 newPos = transform.position;
            newPos.x -= 4;
            transform.position = newPos;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector3 newPos = transform.position;
            newPos.y -= 4;
            transform.position = newPos;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Vector3 newPos = transform.position;
            newPos.x += 4;
            transform.position = newPos;
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate(FireTrap, transform.position, Quaternion.identity);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(SlowTrap, transform.position, Quaternion.identity);
        }
    }
}
