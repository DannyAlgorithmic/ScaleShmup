using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public ShootSystem.BulletType bulletType; // Tipo de bala que este pickup proporcionar�
    public int ammoAmount = 10; // Cantidad de munici�n que a�adir� este pickup

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGameobject = collision.gameObject; // First temporarily cache the collision gameobject
        Transform collisionTransform = collisionGameobject.transform;

        if (!collisionGameobject.CompareTag("Player") || !collisionTransform.root.TryGetComponent(out ShootSystem _shootSystem))
            return; // Verificar si el objeto con el que colisionamos es el jugador

        _shootSystem.AddAmmo(bulletType, ammoAmount); // A�adir la munici�n al tipo espec�fico en el sistema de disparo
        Destroy(gameObject);  // Destruir el pickup despu�s de que se haya recogido
    }
}
