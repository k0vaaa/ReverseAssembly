using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Core.DI
{
    public class DIContainer
    {
        private readonly Dictionary<Type, object> _services = new();

        public void Register<T>(T serviceInstance) where T : class
        {
            Register(typeof(T), serviceInstance);
        }

        public void Register(Type type, object serviceInstance)
        {
            if (!_services.TryAdd(type, serviceInstance)) 
                Debug.LogError($"Service with type {type} can not be registered");
        }

        public T Resolve<T>()
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }

            throw new Exception($"Service with type {typeof(T)} is not registered");
        }

        public void Inject(object target)
        {
            Type targetType = target.GetType();
            FieldInfo[] targetFieldInfo =
                targetType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in targetFieldInfo)
            {
                if (field.GetCustomAttribute<InjectAttribute>() != null)
                {
                    var type = field.FieldType;
                    if (_services.TryGetValue(type, out var service))
                    {
                        field.SetValue(target, service);
                    }
                    else
                    {
                        throw new Exception($"Service with type {type} is not registered");
                    }
                }
            }
        }
    }
}