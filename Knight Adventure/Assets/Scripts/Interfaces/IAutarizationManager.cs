namespace Assets.Scripts.Interfaces
{
    public interface IAutarizationManager
    {
        void StartManager();
        PlayerData GetPlayerData();
        void SetPlayerData(PlayerData playerData);
    }
}
