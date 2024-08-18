using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public float mouseMoveSpeed = 0.1f;
    public bool enableParallax = true;
    public float parallaxAmount = 0.2f;

    private Vector3 offset;
    private Camera mainCamera;

    private void Start()
    {
        offset = transform.position - target.position;
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector2 mousePosition = mainCamera.ScreenToViewportPoint(Input.mousePosition);

        if (mousePosition.x < 0.1f || mousePosition.x > 0.9f || mousePosition.y < 0.1f || mousePosition.y > 0.9f)
        {
            Vector3 mouseDirection = new Vector3(mousePosition.x - 0.5f, mousePosition.y - 0.5f, 0f);
            desiredPosition += mouseDirection * mouseMoveSpeed;
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        if (enableParallax)
        {
            Vector3 parallaxOffset = new Vector3(mousePosition.x - 0.5f, mousePosition.y - 0.5f, 0f) * parallaxAmount;
            transform.position += parallaxOffset;
        }
    }
}
