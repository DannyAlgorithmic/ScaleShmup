using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScalator : MonoBehaviour
{
    public enum BulletType { Shrink, ExpandHorizontal, ExpandVertical }
    public BulletType bulletType;

    public float shrinkSpeed = 5f;
    public float expandSpeed = 5f;
    public float effectDuration = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy != null && !enemy.IsUnderEffect)
            {
                enemy.IsUnderEffect = true;
                Vector3 originalScale = enemy.transform.localScale;

                switch (bulletType)
                {
                    case BulletType.Shrink:
                        StartCoroutine(ScaleEnemy(enemy, originalScale, originalScale * 0.5f, shrinkSpeed));
                        break;
                    case BulletType.ExpandHorizontal:
                        Vector3 newScaleHorizontal = new Vector3(originalScale.x * 3, originalScale.y, originalScale.z);
                        StartCoroutine(ScaleEnemy(enemy, originalScale, newScaleHorizontal, expandSpeed));
                        break;
                    case BulletType.ExpandVertical:
                        Vector3 newScaleVertical = new Vector3(originalScale.x, originalScale.y * 3, originalScale.z);
                        StartCoroutine(ScaleEnemy(enemy, originalScale, newScaleVertical, expandSpeed));
                        break;
                }

                Destroy(gameObject);  // Destruye la bala
            }
        }
    }

    private IEnumerator ScaleEnemy(Enemy enemy, Vector3 originalScale, Vector3 targetScale, float speed)
    {
        float elapsedTime = 0f;
        float scaleFactor = 0f;

        // Escala el objeto al nuevo tamaño
        while (elapsedTime < effectDuration)
        {
            scaleFactor = Mathf.Lerp(1, 0, elapsedTime / effectDuration);
            enemy.transform.localScale = Vector3.Lerp(targetScale, originalScale, scaleFactor);
            elapsedTime += Time.deltaTime * speed;
            yield return null;
        }

        // Vuelve al tamaño original
        elapsedTime = 0f;
        while (elapsedTime < effectDuration)
        {
            scaleFactor = Mathf.Lerp(0, 1, elapsedTime / effectDuration);
            enemy.transform.localScale = Vector3.Lerp(targetScale, originalScale, scaleFactor);
            elapsedTime += Time.deltaTime * speed;
            yield return null;
        }

        enemy.transform.localScale = originalScale;
        enemy.IsUnderEffect = false;  // Permite que el enemigo pueda ser afectado nuevamente
    }
}
