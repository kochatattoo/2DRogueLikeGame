using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ServiceLocator
{
    public class ServiceLocator
    {
        private static Dictionary<Type, object> _services = new Dictionary<Type, object>();


        // Регистрация сервиса
        public static void RegisterService<T>(T service)
        {
            // Проверяется, если сервис уже зарегистрирован
            if (!_services.ContainsKey(typeof(T)))
            {
                _services[typeof(T)] = service;
            }
            else
            {
                throw new Exception($"Service of type {typeof(T)} is already registered.");
            }
        }

        // Получение сервиса
        public static T GetService<T>()
        {
            // Проверяем, зарегистрирован ли сервис
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }
            throw new Exception($"Service of type {typeof(T)} not found.");
        }

        // Удаление сервиса
        public static void UnregisterService<T>()
        {
            if (_services.ContainsKey(typeof(T)))
            {
                _services.Remove(typeof(T));
            }
        }

    }
}
