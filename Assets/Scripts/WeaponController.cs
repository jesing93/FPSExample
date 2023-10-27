using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform outPostion;
    [HeaderAttribute("Ammo")]
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private bool infiniteAmmo;

    [HeaderAttribute("Performance")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float damageMultiplier;
    [SerializeField] private float reloadTime;

    private ObjectPool pool;
    private float lastShootTime;


    private bool isPlayer;

    private void Awake()
    {
        //Check if I'm a player
        if(GetComponent<PlayerManager>())
        {
            isPlayer = true;
        }
        pool = GetComponent<ObjectPool>();
    }

    /// <summary>
    /// Check if it is possible to shoot
    /// </summary>
    /// <returns>Bool</returns>
    public bool CanShoot()
    {
        //Check fire rate
        if (Time.time - lastShootTime >= fireRate)
        {
            //Check ammo
            if (infiniteAmmo || currentAmmo > 0)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Handle weapon shoot
    /// </summary>
    public void Shoot()
    {
        //Update last shoot time
        lastShootTime = Time.time;

        if (!infiniteAmmo)
        {
        }

        //Get a new bullet            //Reduce ammo
            currentAmmo--;

        GameObject bullet = pool.GetGameObject();

        //Set the damage multiplier
        bullet.GetComponent<BulletController>().WeaponMultiplier = damageMultiplier;

        //Locate the bullet at the out position
        bullet.transform.position = outPostion.position;
        bullet.transform.rotation = outPostion.rotation;

        //Give speed to the bullet
        bullet.GetComponent<Rigidbody>().velocity = outPostion.forward * bulletSpeed;
    }

    /// <summary>
    /// Handle the weapon reload
    /// </summary>
    public int Reload(int ammoCount)
    {
        if(currentAmmo < maxAmmo)
        {
            int ammoToReload = maxAmmo - currentAmmo;
            if (ammoCount < ammoToReload)
            {
                currentAmmo += ammoCount;
                return ammoCount;
            }
            else
            {
                currentAmmo = maxAmmo;
                return ammoToReload;
            }
        }
        return 0;
    }
}
