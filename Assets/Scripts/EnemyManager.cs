using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System;

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

    [Header("Patrol")]
    public Transform[] points;
    private int desPoint = 0;

    private PlayerManager target; //Player
    private NavMeshAgent agent;

    //Components
    private WeaponController weapon;

    private void Start()
    {
        target = FindObjectOfType<PlayerManager>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        weapon = GetComponentInChildren<WeaponController>();
        GoToNextPoint();
    }

    private void Update()
    {
        //Search player with raycast
        SearchEnemy();

        //Choose destination point when the agent 
        if (!agent.pathPending && agent.remainingDistance < 1f)
            GoToNextPoint();
    }

    private void SearchEnemy()
    {
        NavMeshHit hit;

        //If there are no obstacles
        if (!agent.Raycast(target.transform.position + Vector3.up, out hit))
        {
            if (hit.distance <= 10f)
            {
                agent.SetDestination(target.transform.position);
                agent.stoppingDistance = 5f;
                transform.LookAt(target.transform.position);

                if (hit.distance <= 6f)
                {
                    if(weapon.CanShoot())
                        weapon.Shoot();
                }
            }
            else
            {
                if (!agent.pathPending) 
                    GoToNextPoint();
            }
        }
    }

    private void GoToNextPoint()
    {
        //Return if there is no ponts to go
        if (points.Length == 0)
            return;

        //Recover stop distance
        agent.stoppingDistance = 0.5f;

        //Set agent to the next point
        agent.destination = points[desPoint].position;

        //Choose the next point in the array
        //cycling to start if neccesary
        desPoint = (desPoint + 1) % points.Length;
    }

    public void DamageEnemy(float quantity, bool isPLayer)
    {
        currentLife -= quantity;
        if(currentLife <= 0)
        {
            Destroy(gameObject);
        }

    }
}
