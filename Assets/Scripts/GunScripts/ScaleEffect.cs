using UnityEngine;

public class ScaleEffect : MonoBehaviour
{
    private Vector3 originalScale;
    private Vector3 targetScale;
    private float effectDuration;
    private float holdDuration;
    private float scaleTime;
    private float elapsedTime = 0f;
    private bool scalingBack = false;
    private bool holdingScale = false;
    private Enemy enemy;

    // Método de inicialización para configurar el efecto de escala
    public void Initialize(Enemy enemy, Vector3 targetScale, float scaleTime, float holdDuration)
    {
        this.enemy = enemy;
        this.originalScale = enemy.transform.localScale;
        this.targetScale = targetScale;
        this.scaleTime = scaleTime;
        this.holdDuration = holdDuration;
        this.effectDuration = scaleTime; // Duración para escalar y regresar al tamaño original
    }

    private void Update()
    {
        if (!holdingScale)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / scaleTime);

            if (!scalingBack)
            {
                // Escalando hacia targetScale
                enemy.transform.localScale = Vector3.Lerp(originalScale, targetScale, progress);
                if (progress >= 1f)
                {
                    // Al llegar a la escala objetivo, comenzamos a mantener la escala
                    holdingScale = true;
                    elapsedTime = 0f;
                }
            }
            else
            {
                // Escalando de vuelta al originalScale
                enemy.transform.localScale = Vector3.Lerp(targetScale, originalScale, progress);
                if (progress >= 1f)
                {
                    // Al volver a la escala original, finalizamos el efecto
                    enemy.transform.localScale = originalScale;
                    enemy.IsUnderEffect = false;
                    Destroy(this);
                }
            }
        }
        else
        {
            // Mantiene la escala objetivo por el tiempo definido en holdDuration
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= holdDuration)
            {
                // Después del tiempo de espera, comenzamos a escalar de vuelta
                holdingScale = false;
                scalingBack = true;
                elapsedTime = 0f;
            }
        }
    }
}
