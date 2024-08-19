using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    public enum BulletType { Default, Type1, Type2, Type3 } // Añade tantos tipos como necesites
    public Bullet bulletDefault, bulletType1, bulletType2, bulletType3; // Referencias a los prefabs de los diferentes tipos de balas
    private BulletType selectedBulletType = BulletType.Default;

    public Transform firePoint;
    public Animator animator;

    [SerializeField] private float cooldownAtaque;
    private float tiempoSiguienteAtaque;

    // Ammo variables
    [SerializeField] private int maxBulletsInClip = 10;
    private int bulletsInClip;
    private bool isReloading;
    [SerializeField] private float reloadTime = 2f;

    // Ammo counts (exposed to the Inspector)
    [SerializeField] private int maxAmmoType1 = 30;
    [SerializeField] private int maxAmmoType2 = 30;
    [SerializeField] private int maxAmmoType3 = 30;

    private Dictionary<BulletType, (int currentAmmo, int maxAmmo)> ammoCounts;


    private void Awake()
    {

        // Initialize ammo counts with the inspector values
        ammoCounts = new Dictionary<BulletType, (int, int)>()
        {
            { BulletType.Default, (int.MaxValue, int.MaxValue) }, // Munición infinita para el tipo Default
            { BulletType.Type1, (0, maxAmmoType1) },
            { BulletType.Type2, (0, maxAmmoType2) },
            { BulletType.Type3, (0, maxAmmoType3) }
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletsInClip = maxBulletsInClip;
        isReloading = false;
        tiempoSiguienteAtaque = 0f;
    }

    // Update is called once per frame
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
                    // Trigger the shoot animation
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
            selectedBulletType = BulletType.Default;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedBulletType = BulletType.Type1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedBulletType = BulletType.Type2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedBulletType = BulletType.Type3;
        }
    }

    private void Shoot()
    {
        Bullet bulletToShoot = GetBulletPrefab(selectedBulletType);
        Bullet theBullet = Instantiate(bulletToShoot, firePoint.position, firePoint.rotation);
        theBullet.shoot(firePoint.up);
        bulletsInClip--;

        // Disminuir la munición disponible para el tipo de bala seleccionado
        if (selectedBulletType != BulletType.Default)
        {
            var ammoData = ammoCounts[selectedBulletType];
            ammoCounts[selectedBulletType] = (ammoData.currentAmmo - 1, ammoData.maxAmmo);
            if (ammoCounts[selectedBulletType].currentAmmo <= 0)
            {
                // Si la munición se acaba, selecciona Default para prevenir errores
                selectedBulletType = BulletType.Default;
            }
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

        // Trigger the reload animation
        animator.SetTrigger("Reload");

        Invoke(nameof(Reload), reloadTime);
    }

    private void Reload()
    {
        var ammoData = ammoCounts[selectedBulletType];

        // Si no hay munición para el tipo seleccionado, recargar con balas predeterminadas
        if (ammoData.currentAmmo <= 0 && selectedBulletType != BulletType.Default)
        {
            selectedBulletType = BulletType.Default;
            ammoData = ammoCounts[selectedBulletType];
        }

        int bulletsToReload = Mathf.Min(maxBulletsInClip, ammoData.currentAmmo);
        bulletsInClip = bulletsToReload;
        ammoCounts[selectedBulletType] = (ammoData.currentAmmo - bulletsToReload, ammoData.maxAmmo);
        isReloading = false;
        animator.SetTrigger("ReloadOff");
    }

    public void AddAmmo(BulletType type, int amount)
    {
        var ammoData = ammoCounts[type];
        ammoCounts[type] = (Mathf.Min(ammoData.currentAmmo + amount, ammoData.maxAmmo), ammoData.maxAmmo);
    }
}
