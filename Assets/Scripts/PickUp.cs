using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField]
    private PickUpType type;
    [SerializeField]
    private int amount;
    [SerializeField]
    private int respawnDelay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (type)
            {
                case PickUpType.health:
                    other.GetComponentInParent<PlayerManager>().ReceiveHealth(amount);
                    break;
                case PickUpType.ammo:
                    other.GetComponentInParent<PlayerManager>().ReceiveAmmo(amount);
                    break;
            }
        }
    }

    public enum PickUpType
    {
        ammo, health
    }
}
