using UnityEngine;

namespace Assets.Scripts.HUD.Windows
{
    public class WarningWindow : Window
    {
        // Вы можете дополнить функционал данного окна
        protected override void OnOkButtonClicked()
        {
            base.OnOkButtonClicked(); // Вызов родительской логики
                                      // Дополнительная логика для окна предупреждения
            Debug.Log("Warning acknowledged!"); // Пример логики
        }
        public static void ShowImportantWarning()
        {
            // Создание экземпляра WarningWindow
            WarningWindow warning = Instantiate(Resources.Load<WarningWindow>("WarningWindowPrefab"));
            ShowPriorityWindow(warning);
        }
        
    }
}
