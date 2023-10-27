using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] private float currentLife;
    [SerializeField] private float maxLife;
    [SerializeField] private int ScorePoints;

    [Header("Enemy Movement")]
    [SerializeField][Range(0, 10)] private float speed;
    [SerializeField] private float attackRange;
    [SerializeField] private float followRange;
    [SerializeField] private bool alwaysFollow;

    public void DamageEnemy(float quantity)
    {
        currentLife -= quantity;
        if(currentLife <= 0)
        {
            Destroy(gameObject);
        }

    }
}
