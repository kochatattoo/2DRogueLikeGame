namespace Assets.Scripts.HUD.Windows
{
    public class InfoWindow : Window
    {
        protected override void OnOkButtonClicked()
        {
            // Логика закрытия окна
            CloseWindow(); // Закрыть окно при нажатии "ОК"
        }

        public override void OpenWindow()
        {
            base.OpenWindow(); // Вызов базового метода OpenWindow
        }
        public void Show()
        {
            QueueWindow(this);
        }
    }
}
