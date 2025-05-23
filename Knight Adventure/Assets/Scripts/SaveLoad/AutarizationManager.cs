using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;
using UnityEngine;

public class AutarizationManager : MonoBehaviour, IManager, IAutarizationManager
{
    // ������ ����� ���� ����������� ����������� � �������� ������, � ����� ���������� �� ����������� ���������
    // � ��������� ������� Player
    public PlayerData _playerData;

    private ISaveManager _saveManager;
    public void StartManager()
    {
        gameObject.SetActive(true);
        _saveManager =ServiceLocator.GetService<ISaveManager>();

        _playerData = _saveManager.LoadLastGame();
    }
    public PlayerData GetPlayerData() => _playerData;
    public void SetPlayerData(PlayerData playerData)
    {
        _playerData = playerData;
    }
    private void HandleData()
    {

    }
}
