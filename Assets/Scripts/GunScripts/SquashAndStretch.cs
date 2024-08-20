using UnityEngine;

public class SquashAndStretch : MonoBehaviour
{
    public float squashFactor = 0.2f;  // Cuánto se comprime
    public float stretchFactor = 0.2f; // Cuánto se estira
    public float animationSpeed = 10f; // Velocidad de la animación

    private Vector3 originalScale;
    private bool isAnimating = false;

    void Start()
    {
        // Guardar la escala original del personaje
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Detectar si se presiona alguna de las teclas WASD
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            isAnimating = true;
        }
        else
        {
            isAnimating = false;
        }

        // Si se está presionando una tecla, reproducir la animación de Squash and Stretch
        if (isAnimating)
        {
            AnimateSquashAndStretch();
        }
        else
        {
            // Si no se presionan teclas, volver a la escala original
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * animationSpeed);
        }
    }

    void AnimateSquashAndStretch()
    {
        // Calcular un valor de escala oscilante entre squash y stretch
        float scaleY = originalScale.y + Mathf.Sin(Time.time * animationSpeed) * squashFactor;
        float scaleX = originalScale.x + Mathf.Sin(Time.time * animationSpeed) * -stretchFactor;

        // Aplicar la nueva escala al personaje
        transform.localScale = new Vector3(scaleX, scaleY, originalScale.z);
    }
}
