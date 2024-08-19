using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    public enum BulletType { Default, Type1, Type2, Type3 }
    public Bullet bulletDefault, bulletType1, bulletType2, bulletType3;
    private BulletType selectedBulletType = BulletType.Default;
    private BulletType lastReloadedBulletType = BulletType.Default; // Guarda el tipo de bala del último recargado

    public Transform firePoint;
    public Animator animator;

    [SerializeField] private float cooldownAtaque;
    private float tiempoSiguienteAtaque;

    [SerializeField] private int maxBulletsInClip = 10;
    private int bulletsInClip;
    private bool isReloading;
    [SerializeField] private float reloadTime = 2f;

    [SerializeField] private int maxAmmoType1 = 30;
    [SerializeField] private int maxAmmoType2 = 30;
    [SerializeField] private int maxAmmoType3 = 30;

    private Dictionary<BulletType, (int currentAmmo, int maxAmmo)> ammoCounts;
    private Dictionary<BulletType, int> bulletsInClipByType;

    private void Awake()
    {
        ammoCounts = new Dictionary<BulletType, (int, int)>()
        {
            { BulletType.Default, (int.MaxValue, int.MaxValue) },
            { BulletType.Type1, (0, maxAmmoType1) },
            { BulletType.Type2, (0, maxAmmoType2) },
            { BulletType.Type3, (0, maxAmmoType3) }
        };

        bulletsInClipByType = new Dictionary<BulletType, int>()
        {
            { BulletType.Default, maxBulletsInClip },
            { BulletType.Type1, 0 },
            { BulletType.Type2, 0 },
            { BulletType.Type3, 0 }
        };
    }

    void Start()
    {
        bulletsInClip = maxBulletsInClip;
        isReloading = false;
        tiempoSiguienteAtaque = 0f;
    }

    void Update()
    {
        HandleBulletSelection();

        if (isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.R) && bulletsInClip < maxBulletsInClip)
        {
            StartReloading();
            return;
        }

        if ((Input.GetButton("Fire1") || Input.GetMouseButton(0)) && tiempoSiguienteAtaque <= 0)
        {
            if (bulletsInClip > 0)
            {
                Shoot();
                tiempoSiguienteAtaque = cooldownAtaque;

                if (!isReloading)
                {
                    animator.SetTrigger("Shoot");
                }
            }
        }

        if (tiempoSiguienteAtaque > 0)
        {
            tiempoSiguienteAtaque -= Time.deltaTime;
        }
    }

    private void HandleBulletSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ChangeBulletType(BulletType.Default);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeBulletType(BulletType.Type1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeBulletType(BulletType.Type2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeBulletType(BulletType.Type3);
        }
    }

    private void ChangeBulletType(BulletType newBulletType)
    {
        if (newBulletType != selectedBulletType)
        {
            // Devuelve las balas restantes al inventario
            ReturnRemainingBulletsToInventory();

            // Cambia el tipo de bala seleccionado
            selectedBulletType = newBulletType;

            // Actualiza las balas en el cargador al tipo seleccionado
            bulletsInClip = bulletsInClipByType[selectedBulletType];
        }
    }

    private void ReturnRemainingBulletsToInventory()
    {
        if (selectedBulletType != BulletType.Default)
        {
            // Devuelve las balas restantes al inventario del tipo actual
            var ammoData = ammoCounts[selectedBulletType];
            ammoCounts[selectedBulletType] = (ammoData.currentAmmo + bulletsInClip, ammoData.maxAmmo);

            // Restablece el número de balas en el cargador del tipo actual a 0
            bulletsInClipByType[selectedBulletType] = 0;
        }
    }

    private void Shoot()
    {
        Bullet bulletToShoot = GetBulletPrefab(selectedBulletType);
        Bullet theBullet = Instantiate(bulletToShoot, firePoint.position, firePoint.rotation);
        theBullet.shoot(firePoint.up);
        bulletsInClip--;

        bulletsInClipByType[selectedBulletType]--;

        if (selectedBulletType != BulletType.Default)
        {
            var ammoData = ammoCounts[selectedBulletType];
            ammoCounts[selectedBulletType] = (ammoData.currentAmmo - 1, ammoData.maxAmmo);
        }
    }

    private Bullet GetBulletPrefab(BulletType type)
    {
        switch (type)
        {
            case BulletType.Type1: return bulletType1;
            case BulletType.Type2: return bulletType2;
            case BulletType.Type3: return bulletType3;
            default: return bulletDefault;
        }
    }

    private void StartReloading()
    {
        if (isReloading || bulletsInClip == maxBulletsInClip)
            return;

        isReloading = true;

        animator.SetTrigger("Reload");

        Invoke(nameof(Reload), reloadTime);
    }

    private void Reload()
    {
        var ammoData = ammoCounts[selectedBulletType];
        int bulletsToReload = Mathf.Min(maxBulletsInClip - bulletsInClip, ammoData.currentAmmo);
        bulletsInClip += bulletsToReload;
        bulletsInClipByType[selectedBulletType] = bulletsInClip;
        ammoCounts[selectedBulletType] = (ammoData.currentAmmo - bulletsToReload, ammoData.maxAmmo);
        isReloading = false;
        lastReloadedBulletType = selectedBulletType;
        animator.SetTrigger("ReloadOff");
    }

    public void AddAmmo(BulletType type, int amount)
    {
        var ammoData = ammoCounts[type];
        ammoCounts[type] = (Mathf.Min(ammoData.currentAmmo + amount, ammoData.maxAmmo), ammoData.maxAmmo);
    }
}
