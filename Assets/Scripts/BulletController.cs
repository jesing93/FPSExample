using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("BulletData")]
    [SerializeField] private float activeTime;
    private float shootTime;
    [SerializeField] private float damage;
    private float weaponMultiplier;

    public float WeaponMultiplier { get => weaponMultiplier; set => weaponMultiplier = value; }

    private void OnEnable()
    {
        shootTime = Time.time;
    }

    private void Update()
    {
        if(Time.time - shootTime >= activeTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            other.GetComponentInParent<EnemyManager>().DamageEnemy(damage);
        }
        //TODO Collision management
    }
}
