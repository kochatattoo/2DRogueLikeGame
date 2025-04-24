using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    private GameObject enemyPrefab;
    private GameObject[] spawnPointsObjects;
    private Transform[] spawnPoints;

    public void StartScript()
    {
        Debug.Log("Start Script");
        spawnPointsObjects = GameObject.FindGameObjectsWithTag("Enemy_Spawn");
        SpawnPointTransformPosition();

        var resourceloadManager = gameObject.AddComponent<ResourcesLoadManager>();
        enemyPrefab = resourceloadManager.LoadEnemyPrefab("Skeleton");

        SpawnEnemies();
    }
    private void SpawnPointTransformPosition()
    {
        spawnPoints = new Transform[spawnPointsObjects.Length];
        for(int i = 0; i < spawnPointsObjects.Length; i++)
        {
            spawnPoints[i]= spawnPointsObjects[i].transform;
        }
    }
    private void SpawnEnemies()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();

            if (agent != null)
            {
                // ”бедитесь, что агент находитс€ на NavMesh
                if (NavMesh.SamplePosition(spawnPoint.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    // Ќачинаем движение к цели
                    Vector3 targetPosition = GetRandomTargetPosition();
                    agent.SetDestination(targetPosition);
                }
                else
                {
                    Debug.LogWarning("Spawn point is not on NavMesh.");
                }
            }
        }
    }

    Vector3 GetRandomTargetPosition()
    {
        // ѕример случайной позиции в границах уровн€
        float x = Random.Range(-2.0f, 2.0f);
        float y = Random.Range(-2.0f, 2.0f);
        return new Vector3(x, y, 0); // ќбратите внимание на Z-координату (она должна совпадать с высотой NavMesh)
    }
}
