using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public ShootSystem.BulletType bulletType; // Tipo de bala que este pickup proporcionar�
    public int ammoAmount = 10; // Cantidad de munici�n que a�adir� este pickup

    private void OnTriggerEnter2D(Collision2D collision)
    {
        // Verificar si el objeto con el que colisionamos es el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Obtener la referencia al ShootSystem del jugador
            ShootSystem shootSystem = collision.transform.parent.GetComponentInParent<ShootSystem>();
            if (shootSystem != null)
            {
                // A�adir la munici�n al tipo espec�fico en el sistema de disparo
                shootSystem.AddAmmo(bulletType, ammoAmount);

                // Destruir el pickup despu�s de que se haya recogido
                Destroy(gameObject);
            }
        }
    }
}
