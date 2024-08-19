using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiarRotacion : MonoBehaviour
{
    public float distanceFromPlayer = 1f;
    public float rotationSpeed = 180f;
    public Transform playerTransform;

    void Update()
    {
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Cambiar la posición Z del arma según la posición del ratón en relación al personaje
        if (cursorPosition.y > playerTransform.position.y)
        {
            cursorPosition.z = 0.1f;
        }
        else
        {
            cursorPosition.z = -0.1f;
        }

        Vector3 direction = cursorPosition - playerTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotar el sprite en 90 grados en sentido contrario a las agujas del reloj (-90 grados)
        angle -= 90f;

        Quaternion desiredRotation = Quaternion.Euler(0f, 0f, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        transform.position = playerTransform.position + direction.normalized * distanceFromPlayer;

        // Cambiar la escala en el eje X del arma según la posición del ratón en relación al personaje
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
        }
    }
}
