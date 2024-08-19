using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public ShootSystem.BulletType bulletType; // Tipo de bala que este pickup proporcionará
    public int ammoAmount = 10; // Cantidad de munición que añadirá este pickup

    private void OnTriggerEnter2D(Collision2D collision)
    {
        // Verificar si el objeto con el que colisionamos es el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Obtener la referencia al ShootSystem del jugador
            ShootSystem shootSystem = collision.transform.parent.GetComponentInParent<ShootSystem>();
            if (shootSystem != null)
            {
                // Añadir la munición al tipo específico en el sistema de disparo
                shootSystem.AddAmmo(bulletType, ammoAmount);

                // Destruir el pickup después de que se haya recogido
                Destroy(gameObject);
            }
        }
    }
}
