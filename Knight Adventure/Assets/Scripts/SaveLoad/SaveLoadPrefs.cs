using Assets.ServiceLocator;
using UnityEngine;
using Assets.Scripts.Interfaces;

public class SaveLoadPrefs : MonoBehaviour
{

    //������ ��� ������������ ��� ���������� ���������������� ��������
    //����� ��� �������, ���� � ������ ������������
    //��������� ������� ������ ����� ���������� ����� ������������ ����� 
    string _userName = "";
    int _userlvl = 1;
    int _userCoins = 1;
    float _userPositionX = 0.0f;
    float _userPositionY=0.0f;

    float _playerHealth = Player.Instance.CurrentHealth;


    public void SaveGame()
    {
        GetUserData();

        PlayerPrefs.SetString("SavedName", _userName);
        PlayerPrefs.SetInt("SavedCoins", _userCoins);
        PlayerPrefs.SetInt("SavedLvl", _userlvl);
        PlayerPrefs.SetFloat("SavedPositionX", _userPositionX);
        PlayerPrefs.SetFloat("SavedPositionY", _userPositionY);
        PlayerPrefs.SetFloat("SavedHealth",_playerHealth);

        PlayerPrefs.Save();
        Debug.Log("Game data saved");
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("SavedInteger"))
        {
            _userName = PlayerPrefs.GetString("SavedName");
            _userCoins = PlayerPrefs.GetInt("SavedCoins");
            _userCoins = PlayerPrefs.GetInt("Savedlvl");
            _userPositionX = PlayerPrefs.GetFloat("SavedPositionX");
            _userPositionX = PlayerPrefs.GetFloat("SavedPositionY");
            _playerHealth = PlayerPrefs.GetFloat("SavedHelth");
            Debug.Log("Game data loaded!");
        }
        else
            Debug.LogError("There is no save data!");
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        _userName = "Non";
        _userCoins = 0;
        _userlvl = 1;
        _userPositionX = 0.0f;
        _userPositionY = 0.0f;
        _playerHealth=Player.Instance.MaxHealth;
        Debug.Log("Data reset complete");
    }

    private void GetUserData()
    {
        var autarizationManager = ServiceLocator.GetService<IAutarizationManager>();
        _userName = autarizationManager.GetPlayerData().name;
        _userlvl = autarizationManager.GetPlayerData().level;
        _userCoins = autarizationManager.GetPlayerData().coins;
        _userPositionX = 0.0f;
        _userPositionY = 0.0f;
    }
}
