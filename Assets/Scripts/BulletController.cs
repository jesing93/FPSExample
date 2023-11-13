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
    [SerializeField] private GameObject damageParticle;
    [SerializeField] private GameObject impactParticle;

    private bool isPlayer;

    public float WeaponMultiplier { get => weaponMultiplier; set => weaponMultiplier = value; }
    public bool IsPlayer { get => isPlayer; set => isPlayer = value; }

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
            other.GetComponentInParent<EnemyManager>().DamageEnemy(damage, isPlayer);
            GameObject particles = Instantiate(damageParticle, transform.position, Quaternion.identity);
            if (isPlayer)
            {
                PlayerManager.instance.AddScore(10);
            }
        }
        else if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            other.GetComponentInParent<PlayerManager>().ReceiveDamage(damage, isPlayer);
            GameObject particles = Instantiate(damageParticle, transform.position, Quaternion.identity);
        }
        else
        {
            GameObject particles = Instantiate(impactParticle, transform.position, Quaternion.identity);
        }
        this.enabled = false;
    }
}
