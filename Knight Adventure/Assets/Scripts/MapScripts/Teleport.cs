using Assets.ServiceLocator;
using UnityEngine;
using Assets.Scripts.Interfaces;

public class Teleport : MonoBehaviour
{
    public int mapIndex; // ������ ����� ��� ��������

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("I'm here");

        if (collision.CompareTag("Player"))
        {
            var mapManager = ServiceLocator.GetService<IMapManager>();
            mapManager.LoadMap(mapIndex); // ������� � ��������������� �����
            
        }
    }
}
