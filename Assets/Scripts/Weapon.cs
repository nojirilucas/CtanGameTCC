using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    public int weaponDamage;

    [Header("Shooting")]
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    [Header("Burst")]
    public int bulletsPerBurst = 3;
    public int burstBulletLeft;

    [Header("Spread")]
    public float spreadIntensity;
    public float hipSpreadIntensity;
    public float adsSpreadIntensity;

    public GameObject muzzleEffect;
    internal Animator animator;

    [Header("Flashlight")]
    public GameObject lightSource;
    private bool isLightOn = false;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;
    public Vector3 spawnScale;

    bool inAIM;

    public enum WeaponModel
    {
        Bereta,
        Taser,
        Flashlight
    }

    public WeaponModel thisWeaponModel;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();
        spreadIntensity = hipSpreadIntensity;

        if (thisWeaponModel == WeaponModel.Flashlight && lightSource != null)
        {
            lightSource.SetActive(isLightOn);
        }
    }

    void Update()
    {
        if (!isActiveWeapon)
        {
            foreach (Transform t in GetComponentsInChildren<Transform>(true))
            {
                t.gameObject.layer = LayerMask.NameToLayer("Default");
            }
            return;
        }

        foreach (Transform t in GetComponentsInChildren<Transform>(true))
        {
            t.gameObject.layer = LayerMask.NameToLayer("WeaponRender");
        }

        if (thisWeaponModel == WeaponModel.Flashlight)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                isLightOn = !isLightOn;
                lightSource.SetActive(isLightOn);
            }
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            EnterADS();
        }

        if (Input.GetMouseButtonUp(1))
        {
            ExitADS();
        }

        if (currentShootingMode == ShootingMode.Auto)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (currentShootingMode == ShootingMode.Single ||
                 currentShootingMode == ShootingMode.Burst)
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (readyToShoot && isShooting && inAIM)
        {
            burstBulletLeft = bulletsPerBurst;
            FireWeapon();
        }
    }

    private void EnterADS()
    {
        animator.SetTrigger("inAIM");
        animator.ResetTrigger("exitAIM");
        inAIM = true;
        spreadIntensity = adsSpreadIntensity;
    }

    private void ExitADS()
    {
        animator.SetTrigger("exitAIM");
        animator.ResetTrigger("inAIM");
        inAIM = false;
        spreadIntensity = hipSpreadIntensity;
    }

    private void FireWeapon()
    {
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOILAIM");
        readyToShoot = false;

        if (allowReset)
        {
            StartCoroutine(ResetShotCoroutine());
            allowReset = false;
        }

        if (currentShootingMode == ShootingMode.Burst && burstBulletLeft > 1)
        {
            burstBulletLeft--;
            StartCoroutine(FireWeaponCoroutine());
        }
    }

    private IEnumerator ResetShotCoroutine()
    {
        yield return new WaitForSeconds(shootingDelay);
        readyToShoot = true;
        allowReset = true;
    }

    private IEnumerator FireWeaponCoroutine()
    {
        yield return new WaitForSeconds(shootingDelay);
        FireWeapon();
    }
}