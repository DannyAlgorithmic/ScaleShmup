using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerPickups : MonoBehaviour
{
    public ShootSystem shootSystem;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGameobject = collision.gameObject;
        Transform collisionTransform = collisionGameobject.transform;

        if (!collisionGameobject.CompareTag("Pickup")) return;

        if (collisionTransform.TryGetComponent(out AmmoPickup _ammoPickup))
        {
            shootSystem.AddAmmo(_ammoPickup.bulletType, _ammoPickup.ammoAmount);
            shootSystem.ChangeBulletType(_ammoPickup.bulletType);
            shootSystem.StartReloading();
            _ammoPickup.selfGameObject.SetActive(false);
        }
    }
}