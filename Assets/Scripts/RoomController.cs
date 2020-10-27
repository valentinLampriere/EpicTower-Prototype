using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private GameObject roomPreviewPrefab = null;
    [SerializeField] private float width = 0;
    [SerializeField] private float height = 0;

    private GameObject roomPreviewGO;
    private Camera mainCamera;
    private CapsuleCollider[] verticesColliders;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            roomPreviewGO = Instantiate(roomPreviewPrefab, GetMousePositionInWorld(), Quaternion.identity);
            verticesColliders = roomPreviewGO.GetComponents<CapsuleCollider>();
        }

        if (roomPreviewGO != null)
        {
            roomPreviewGO.transform.position = GetMousePositionInWorld();
            CheckSnapping();
        }
    }

    Vector3 GetMousePositionInWorld()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
        Input.mousePosition.y, -mainCamera.gameObject.transform.position.z));
        return mousePosition;
    }

    void CheckSnapping()
    {
    }
}
