using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;
    private Vector3 offset;

    public float moveSpeed = 15f;
    public float rotationSpeed = 5f;

    private Transform cameraTarget;

    void Start()
    {
        target = Object.FindAnyObjectByType<PlayerController>().transform;

        offset = transform.position - target.position;

        //Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        // Seguir al jugador
        transform.position = Vector3.Lerp(
            transform.position,
            target.position + offset,
            moveSpeed * Time.deltaTime
        );

        // Girar la cámara si hay un nuevo punto
        if (cameraTarget != null)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                cameraTarget.rotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    public void ChangeCameraDirection(Transform newCameraTarget)
    {
        cameraTarget = newCameraTarget;
    }
}