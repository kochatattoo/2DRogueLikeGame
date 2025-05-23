﻿using System;
using System.Collections.Generic;

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
                // throw new Exception($"Service of type {typeof(T)} is already registered.");
                if (_services[typeof(T)] is UnityEngine.MonoBehaviour newService)
                {
                    UnityEngine.Object.Destroy(newService.gameObject);
                }
                return;
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
