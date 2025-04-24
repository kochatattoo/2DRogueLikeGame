using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus;
using UnityEngine.UI;

public class MapStartScript : MonoBehaviour
{
    [SerializeField] private GameObject _playerInstatePositionObject;  
    public void StartScript()
    {
        var playerInstancePosition = _playerInstatePositionObject.transform.position;
        Player.Instance.SetPlayerPosition(playerInstancePosition);

        SpawnEnemies();     

    }

    private void SpawnEnemies()
    {
        var enemySpawner = FindObjectOfType<EnemySpawner>();
        if (enemySpawner != null)
        {
            enemySpawner.GetComponent<EnemySpawner>().StartScript();
        }
        else
        {
            Debug.Log("There is no spawner");
        }
    }

}
