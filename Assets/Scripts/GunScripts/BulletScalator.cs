using UnityEngine;

public class BulletScalator : MonoBehaviour
{
    public enum BulletType { Shrink, ExpandHorizontal, ExpandVertical }
    public BulletType bulletType;

    public float shrinkAmount = 0.5f;
    public float expandAmount = 3f;
    public float scaleTime = 1f;  // Tiempo para llegar a la escala objetivo
    public float holdDuration = 2f;  // Tiempo para mantener la escala objetivo

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.transform.root.GetComponent<Enemy>();

            if (enemy != null && !enemy.IsUnderEffect)
            {
                enemy.IsUnderEffect = true;

                Vector3 targetScale = enemy.transform.localScale;

                switch (bulletType)
                {
                    case BulletType.Shrink:
                        targetScale *= shrinkAmount;
                        break;
                    case BulletType.ExpandHorizontal:
                        targetScale.x *= expandAmount;
                        break;
                    case BulletType.ExpandVertical:
                        targetScale.y *= expandAmount;
                        break;
                }

                // Añade el script de escalado dinámico al enemigo
                ScaleEffect scaleEffect = enemy.transform.root.gameObject.AddComponent<ScaleEffect>();
                scaleEffect.Initialize(enemy, targetScale, scaleTime, holdDuration);

                // Destruye la bala
                Destroy(gameObject);
            }
        }
    }
}
