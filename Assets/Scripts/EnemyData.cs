using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy/Data", order = 0)]
public class EnemyData : ScriptableObject
{
    [SerializeField] private string enemyName;
    [SerializeField] private string enemyDescription;
    [SerializeField] private float speed;
    [SerializeField] private float fireRate;
    [SerializeField] private Material enemyMaterial;
    [SerializeField] private float maxLife;

    public string EnemyName { get => enemyName; }
    public string EnemyDescription { get => enemyDescription; }
    public float Speed { get => speed; }
    public float FireRate { get => fireRate; }
    public Material EnemyMaterial { get => enemyMaterial; }
    public float MaxLife { get => maxLife; }
}
