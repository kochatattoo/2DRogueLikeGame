using Assets.Scripts.Interfaces;
using Assets.ServiceLocator;
using UnityEngine;

public class AutarizationManager : MonoBehaviour, IAutarizationManager
{
    // Данный класс будт отслеживать авторизацию и загрузку данных, а после передавать ее необходимым элементам
    // В частности объекту Player
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
