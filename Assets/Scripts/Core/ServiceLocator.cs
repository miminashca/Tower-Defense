using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple Service Locator pattern implementation that stores and retrieves 
/// instances of various types. This allows global access to services without 
/// direct singleton references for each individual manager.
/// </summary>
public static class ServiceLocator
{
    /// <summary>
    /// A dictionary holding service instances keyed by their System.Type.
    /// </summary>
    private static Dictionary<Type, object> services = new Dictionary<Type, object>();
    
    /// <summary>
    /// Registers a service of type T into the locator. 
    /// Subsequent calls to Get&lt;T&gt;() can retrieve this instance.
    /// </summary>
    /// <typeparam name="T">The concrete type of the service.</typeparam>
    /// <param name="service">An instance of the service to be stored.</param>
    public static void Register<T>(T service)
    {
        services[typeof(T)] = service;
    }
    
    /// <summary>
    /// Retrieves the registered service of type T from the locator. 
    /// Logs a warning if no instance of that type is found.
    /// </summary>
    /// <typeparam name="T">The type of the requested service.</typeparam>
    /// <returns>The service instance if found; otherwise, the default value of T.</returns>
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