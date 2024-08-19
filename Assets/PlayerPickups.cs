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

        if (!collisionGameobject.CompareTag("Pickup"))
            return;

        if (collisionTransform.TryGetComponent(out AmmoPickup _ammoPickup))
        {
            shootSystem.AddAmmo(_ammoPickup.bulletType, _ammoPickup.ammoAmount);
            _ammoPickup.selfGameObject.SetActive(false);
        }
    }
}

/*
    public int InitialAmmoCount;
    public int MaximumAmmoCount;
    ObjectPool<AmmoPickup> ammoPool;

    private void Awake()
    {
        ammoPool = new ObjectPool<AmmoPickup>(CreateAmmo, InitialAmmoCount, MaximumAmmoCount);
    }

    public AmmoPickup CreateAmmo()
    {

            Debug.Log($"Ammo - Active: {ammoPool.CountActive}, Inactive: {ammoPool.CountInactive}, All: {ammoPool.CountAll}");
    }
*/