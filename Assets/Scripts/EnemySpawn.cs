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

        if(GameObject.FindGameObjectsWithTag("Enemy").Length >= enemyNumber)
        {
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length < enemyNumber);
            spawnCoroutine = StartCoroutine(Spawn());
        }
        else
        {
            spawnCoroutine = StartCoroutine(Spawn());
        }
    }

    private void InstantiateEnemy()
    {
        Instantiate(enemyObject, RandomNavMeshLocation(), Quaternion.identity);
    }

    /// <summary>
    /// Generate a random location in the area
    /// </summary>
    /// <returns></returns>
    private Vector3 RandomNavMeshLocation()
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

    /// <summary>
    /// Generate a random location in the area
    /// </summary>
    /// <returns></returns>
    private Vector3 RandomLocation()
    {
        float positionX = Random.Range(transform.position.x - 15, transform.position.x + 5);
        float positionZ = Random.Range(transform.position.z - 15, transform.position.z + 5);

        return new Vector3(positionX, 1f, positionZ);
    }
}
