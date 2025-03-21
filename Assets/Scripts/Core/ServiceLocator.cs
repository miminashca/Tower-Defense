using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static Dictionary<Type, object> services = new Dictionary<Type, object>();
    
    public static void Register<T>(T service)
    {
        services[typeof(T)] = service;
    }
    
    public static T Get<T>()
    {
        if (services.TryGetValue(typeof(T), out object service))
        {
            return (T)service;
        }
        Debug.LogWarning($"Service {typeof(T)} not found!");
        return default;
    }
}