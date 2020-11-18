using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float ZoomRange = 0f;

    private Vector3 previousPosition;
    private bool zoomedIn = false;
    private float minFov = -25f;
    private float maxFov = -10f;
    public float mouseSensitivity = 10f;

    private bool lockPlayer;

    [HideInInspector]
    public Transform trPlayer;
    Vector3 positionSaved;

    private void Start()
    {
        lockPlayer = false;
        positionSaved = transform.position;
    }

    private void Update()
    {
        float zoom = 0;
        zoom += Input.GetAxis("Mouse ScrollWheel") * mouseSensitivity;
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z + zoom, minFov, maxFov));

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime * 10;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * 10;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            lockPlayer = !lockPlayer;
            if (!lockPlayer)
            {
                transform.position = positionSaved;
            }
            else
            {
                positionSaved = transform.position;
                transform.position = new Vector3(transform.position.x, transform.position.y, maxFov);
            }

        }

        if (lockPlayer)
        {
            transform.position = new Vector3(trPlayer.position.x, trPlayer.position.y, transform.position.z);
        }
        else if (Input.GetMouseButton(1))
        {
            transform.position = new Vector3(transform.position.x - mouseX, transform.position.y - mouseY, transform.position.z);
        }
    }

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