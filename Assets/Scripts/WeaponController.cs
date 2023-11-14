using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [SerializeField] private WeaponType weaponType;

    //Components
    [SerializeField] private GameObject currentWeapon;
    [SerializeField] private GameObject[] weaponsList;

    private bool isPlayer = false;

    public bool IsPlayer { get => isPlayer; set => isPlayer = value; }

    private void Awake()
    {
        //Check if I'm a player
        if(GetComponent<PlayerManager>())
        {
            IsPlayer = true;
            HudController.instance.UpdateCurrentAmmo(currentAmmo + " / " + maxAmmo);
        }
        pool = GetComponent<ObjectPool>();
        /*
        switch (weaponType)
        {
            case WeaponType.bazooka:
                currentWeapon = weaponsList[1];
                outPostion = currentWeapon.transform.Find("OutPosition").transform;
                break;
            default: 
                
        currentWeapon = weaponsList[0];
                break;
        }*/
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
            //Reduce ammo
            currentAmmo--;
            if (isPlayer)
            {
                HudController.instance.UpdateCurrentAmmo(currentAmmo + " / " + maxAmmo);
            }
        }

        //Get a new bullet            

        GameObject bullet = pool.GetGameObject();

        //Set owner of the bullet
        bullet.GetComponent<BulletController>().IsPlayer = isPlayer;

        //Set the damage multiplier
        bullet.GetComponent<BulletController>().WeaponMultiplier = damageMultiplier;

        //Locate the bullet at the out position
        bullet.transform.position = outPostion.position;
        bullet.transform.rotation = outPostion.rotation;

        if (IsPlayer)
        {
            //Create ray from the camera to the target
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            Vector3 targetPoint;

            //Check if you are pointing to sth and adjust the direction
            if (Physics.Raycast(ray, out hit))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(5); //5m

            //Give speed to the bullet
            bullet.GetComponent<Rigidbody>().velocity = (targetPoint - bullet.transform.position).normalized * bulletSpeed;
        }
        else
        {
            //Give speed to the bullet
            bullet.GetComponent<Rigidbody>().velocity = outPostion.forward * bulletSpeed;
        }
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
                HudController.instance.UpdateCurrentAmmo(currentAmmo + " / " + maxAmmo);
                return ammoCount;
            }
            else
            {
                currentAmmo = maxAmmo;
                HudController.instance.UpdateCurrentAmmo(currentAmmo + " / " + maxAmmo);
                return ammoToReload;
            }
            
        }
        return 0;
    }
}
public enum WeaponType
{
    pistol, bazooka
}
