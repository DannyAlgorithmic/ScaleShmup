using System.Collections.Generic;
using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    public enum BulletType { Default, Type1, Type2, Type3 }
    public Bullet bulletDefault, bulletType1, bulletType2, bulletType3;
    private BulletType selectedBulletType = BulletType.Default;
    private BulletType lastReloadedBulletType = BulletType.Default;

    public Transform firePoint;
    public Animator animator;

    [SerializeField] private SpriteRenderer weaponSpriteRenderer;
    [SerializeField] private Sprite defaultWeaponSprite, weaponType1Sprite, weaponType2Sprite, weaponType3Sprite;

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
        UpdateWeaponSprite();
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
                    PlayShootAnimation();
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeBulletType(BulletType.Default);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeBulletType(BulletType.Type1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeBulletType(BulletType.Type2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeBulletType(BulletType.Type3);
        }
    }

    private void ChangeBulletType(BulletType newBulletType)
    {
        if (ammoCounts[newBulletType].currentAmmo > 0 || newBulletType == BulletType.Default)
        {
            if (newBulletType != selectedBulletType)
            {
                ReturnRemainingBulletsToInventory();
                selectedBulletType = newBulletType;
                bulletsInClip = bulletsInClipByType[selectedBulletType];
                UpdateWeaponSprite();
            }
        }
        else
        {
            Debug.Log("No tienes munición de este tipo.");
        }
    }

    private void ReturnRemainingBulletsToInventory()
    {
        if (selectedBulletType != BulletType.Default)
        {
            var ammoData = ammoCounts[selectedBulletType];
            ammoCounts[selectedBulletType] = (ammoData.currentAmmo + bulletsInClip, ammoData.maxAmmo);
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

        if (ammoCounts[selectedBulletType].currentAmmo > 0 || selectedBulletType == BulletType.Default)
        {
            isReloading = true;

            switch (selectedBulletType)
            {
                case BulletType.Type1:
                    animator.SetTrigger("ReloadType1");
                    break;
                case BulletType.Type2:
                    animator.SetTrigger("ReloadType2");
                    break;
                case BulletType.Type3:
                    animator.SetTrigger("ReloadType3");
                    break;
                default:
                    animator.SetTrigger("ReloadDefault");
                    break;
            }

            Invoke(nameof(Reload), reloadTime);
        }
        else
        {
            Debug.Log("No tienes munición para recargar.");
        }
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

    private void UpdateWeaponSprite()
    {
        switch (selectedBulletType)
        {
            case BulletType.Type1:
                weaponSpriteRenderer.sprite = weaponType1Sprite;
                break;
            case BulletType.Type2:
                weaponSpriteRenderer.sprite = weaponType2Sprite;
                break;
            case BulletType.Type3:
                weaponSpriteRenderer.sprite = weaponType3Sprite;
                break;
            default:
                weaponSpriteRenderer.sprite = defaultWeaponSprite;
                break;
        }
    }

    private void PlayShootAnimation()
    {
        switch (selectedBulletType)
        {
            case BulletType.Type1:
                animator.SetTrigger("ShootType1");
                break;
            case BulletType.Type2:
                animator.SetTrigger("ShootType2");
                break;
            case BulletType.Type3:
                animator.SetTrigger("ShootType3");
                break;
            default:
                animator.SetTrigger("ShootDefault");
                break;
        }
    }

    public bool HasAmmoForType(int bulletTypeNumber)
    {
        BulletType bulletType = (BulletType)(bulletTypeNumber - 1); // Convertir el número al tipo de bala
        return ammoCounts[bulletType].currentAmmo > 0 || bulletType == BulletType.Default;
    }
}
