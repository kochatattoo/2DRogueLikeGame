using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;
using UnityEngine;

public class AutarizationManager : MonoBehaviour, IAutarizationManager
{
    // ������ ����� ���� ����������� ����������� � �������� ������, � ����� ���������� �� ����������� ���������
    // � ��������� ������� Player
    private PlayerData _playerData;

    private ISaveManager _saveManager;
    public void StartManager()
    {
        _saveManager=ServiceLocator.GetService<ISaveManager>();

        _playerData = _saveManager.LoadLastGame();
    }
    public PlayerData GetPlayerData() => _playerData;
    private void HandleData()
    {

    }
}
