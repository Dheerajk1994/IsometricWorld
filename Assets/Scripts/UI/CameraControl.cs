using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Camera))]
public class CameraControl : MonoBehaviour
{
    [SerializeField] private float cameraMoveSpeed = 5f;
    [SerializeField] private float cameraZoomSpeed = 5f;
    [SerializeField] private float maxZoomInLevel = 3f;
    [SerializeField] private float maxZoomOutLevel = 6f;

    private Camera camera;

    private void Start()
    {
        camera = this.GetComponent<Camera>();
    }

    private void Update()
    {
        CheckForCameraMovement();
        CheckForCameraZoom();
    }

    private void CheckForCameraMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        this.transform.position += new Vector3(horizontal, vertical).normalized * Time.deltaTime * cameraMoveSpeed;
    }

    private void CheckForCameraZoom()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && camera.orthographicSize >= maxZoomInLevel)
        {
            camera.orthographicSize -= cameraZoomSpeed;
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0 && camera.orthographicSize <= maxZoomOutLevel)
        {
            camera.orthographicSize += cameraZoomSpeed;
        }
    }
}
