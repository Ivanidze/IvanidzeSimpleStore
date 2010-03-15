﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using System.ComponentModel;
using Castle.Core.Interceptor;

namespace Infrasturcture.DataBindingInterception
{
    /// <summary>
    /// фабрика для датабиндинга
    /// </summary>
    public static class DataBindingFactory
    {
        private static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();
        public static T Create<T>()
        {
            return (T) Create(typeof (T));
        }

        public static object Create(Type type)
        {
            return ProxyGenerator.CreateClassProxy(type,
                                                   new[] {typeof (INotifyPropertyChanged), typeof (IMarkerInterface)},
                                                   new NotifyPropertyChangedInterceptor(type.FullName));
        }
        public interface IMarkerInterface
        {
            string TypeName { get; }
        }
        /// <summary>
        /// Класс перехватчик NotifyPropertyChanged к сущности 
        /// </summary>
        public class NotifyPropertyChangedInterceptor:IInterceptor
        {
            private readonly string _typeName;
            private PropertyChangedEventHandler _subscribers = delegate { };
            public NotifyPropertyChangedInterceptor(string typeName)
            {
                _typeName = typeName;
            }

            public void Intercept(IInvocation invocation)
            {
                if (invocation.Method.DeclaringType==typeof(IMarkerInterface))
                {
                    invocation.ReturnValue = _typeName;
                    return;
                }
                if (invocation.Method.DeclaringType== typeof(INotifyPropertyChanged))
                {
                    var propertyChangedEventHandler = (PropertyChangedEventHandler) invocation.Arguments[0];
                    if (invocation.Method.Name.StartsWith("add_"))
                    {
                        _subscribers += propertyChangedEventHandler;
                    }
                    else
                    {
                        _subscribers -= propertyChangedEventHandler;
                    }
                    return;
                }
                invocation.Proceed();
                if (invocation.Method.Name.StartsWith("set_"))
                {
                    var propertyName = invocation.Method.Name.Substring(4);
                    _subscribers(invocation.InvocationTarget,new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}
