using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleMania
{
    public static class Locator
    {
        private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        public static bool HasService<T>()
        {
            Type serviceType = typeof(T);
            return services.ContainsKey(serviceType);
        }
        public static bool HasService(Type serviceType)
        {
            return services.ContainsKey(serviceType);
        }

        public static void RegisterService<T>(T service)
        {
            Type serviceType = typeof(T);

            if (!services.ContainsKey(serviceType))
            {
                services.Add(serviceType, service);
            }
            else
            {
                Debug.LogWarning($"Service of type {serviceType} is already registered. Replacing reference.");
                services[serviceType] = service;
            }
        }

        public static T GetService<T>()
        {
            Type serviceType = typeof(T);

            if (services.TryGetValue(serviceType, out object service))
            {
                return (T)service;
            }
            else
            {
                throw new KeyNotFoundException($"Service of type {serviceType} not registered.");
            }
        }
    }
}