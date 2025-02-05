using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IGUIManager
    {
        //Метод для старта менеджера  
        void StartManager();
        void AddQueueWindows();
        void SetTextAreas();
        void OpenPlayerWindow(GameObject name);
        void OpenPlayerWindow(string name);
        void OpenInformationWindow(int windowIndex);
        void ShowWindowQueue();
        void OpenPriorityWindow(int windowIndex);
        void HandleError(string errorMessage, int numberOfError);
        void CloseCurrentWindow();
        void OpenOption();
        void OpenStorageChestInventory(Inventory chestInventory);

    }
}
