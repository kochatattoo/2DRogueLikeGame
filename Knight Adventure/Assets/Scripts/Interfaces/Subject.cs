using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    public class Subject
    {
        private List<IObserver> observers = new List<IObserver>();

        // Метод для добавления наблюдателя
        public void RegisterObserver(IObserver observer)
        {
            observers.Add(observer);
        }

        // Метод для удаления наблюдателя
        public void UnregisterObserver(IObserver observer)
        {
            observers.Remove(observer);
        }

        // Метод для уведомления всех наблюдателей
        public void NotifyObservers(string message)
        {
            foreach (var observer in observers)
            {
                observer.OnNotify(message);
            }
        }
    }
}
