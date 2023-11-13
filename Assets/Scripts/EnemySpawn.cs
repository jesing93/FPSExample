using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private int enemyNumber;
    [SerializeField] private int spawnNumber;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private GameObject enemyObject;
    [SerializeField] private float radius;

    private Coroutine spawnCoroutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(Spawn());
        }
    }

    private IEnumerator Spawn()
    {
        InstantiateEnemy();
        yield return new WaitForSeconds(1.5f);

        //if(GameObject.)
    }

    private void InstantiateEnemy()
    {
        Instantiate(enemyObject, RandomLocation(), Quaternion.identity);
    }

    /// <summary>
    /// Generate a random location in the area
    /// </summary>
    /// <returns></returns>
    private Vector3 RandomLocation()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        int count = 0;
        do
        {
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                finalPosition = hit.position;
            }
            count++;
            if (count > 10)
            {
                Debug.Log("Break");
                break;
            }
        } while (finalPosition != null);

        return finalPosition;
    }
}
