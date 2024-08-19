using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothRotationMouse : MonoBehaviour
{
    public float rotationSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        transform.up = Vector2.Lerp(transform.up, direction, rotationSpeed * Time.deltaTime);
    }
}
