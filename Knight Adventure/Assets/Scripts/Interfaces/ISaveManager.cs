using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    public interface ISaveManager
    {
        void StartManager();
        void SaveGame(PlayerData playerData, string fileName);
        void QuickSaveGame(PlayerData playerData);
        PlayerData LoadGame(string fileName);
        PlayerData LoadLastGame();
        List<string> GetSaveFileNames();
        void DeleteSaveFile(string fileName);




    }
}
