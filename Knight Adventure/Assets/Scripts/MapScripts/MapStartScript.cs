using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStartScript : MonoBehaviour
{
    [SerializeField] private GameObject _playerInstatePositionObject;
   private void Start()
    {
        var playerInstancePosition = _playerInstatePositionObject.transform.position;
        Player.Instance.SetPlayerPosition(playerInstancePosition);
    }

    public void StartScript()
    {
        var playerInstancePosition = _playerInstatePositionObject.transform.position;
        Player.Instance.SetPlayerPosition(playerInstancePosition);
    }
}
