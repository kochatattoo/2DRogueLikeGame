using UnityEngine;

public class Teleport : MonoBehaviour
{
    public int mapIndex; // ������ ����� ��� ��������

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("I'm here");

        if (collision.CompareTag("Player"))
        {
            MapManager guiManager = FindObjectOfType<MapManager>();

            //MapManager guiManager=MapManager.Instance;
            guiManager.LoadMap(mapIndex); // ������� � ��������������� �����
            
        }
    }
}
