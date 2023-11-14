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
    private bool respawning = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !respawning)
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
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        respawning = true;
        this.transform.position -= new Vector3 (0f, 100f, 0f);
        yield return new WaitForSeconds(5f);
        respawning = false;
        this.transform.position += new Vector3 (0f, 100f, 0f);
    }

    public enum PickUpType
    {
        ammo, health
    }
}
