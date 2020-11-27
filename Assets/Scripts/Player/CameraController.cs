﻿using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float ZoomRange = 0f;

    private Vector3 previousPosition;
    private bool zoomedIn = false;

    public void ZoomIn(Vector3 zoomPosition)
    {
        if (!zoomedIn)
        {
            zoomedIn = true;
            previousPosition = transform.position;

            Vector3 direction = (zoomPosition - transform.position).normalized;

            transform.position += direction * ZoomRange;
        }
        else
        {
            zoomPosition.z -= ZoomRange;
            transform.position = zoomPosition;
        }
    }

    public void ZoomOut()
    {
        if (zoomedIn)
        {
            zoomedIn = false;
            transform.position = previousPosition;
        }
    }
}