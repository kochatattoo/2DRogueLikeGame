namespace Assets.Scripts.Interfaces
{
    public interface INotificationManager
    {
        void OpenNotificationWindow(string name);
        void PlayNotificationAudio(string name);
        void StartManager();
        void HandleError(string errorMessage, int numberOfError);

    }
}
