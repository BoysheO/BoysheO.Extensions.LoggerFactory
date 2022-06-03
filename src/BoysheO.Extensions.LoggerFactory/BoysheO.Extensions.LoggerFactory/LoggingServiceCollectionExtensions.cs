// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using BoysheO.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace BoysheO.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up logging services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class LoggingServiceCollectionExtensions
    {
        /// <summary>
        /// 由于IL2Cpp不实现CustomAttributeData.GetCustomAttributes，所以需要另外提供这个函数的实现
        /// 如果不需要支持ProviderAliasAttribute别名属性，可以直接返回null
        /// GetAliasFunc原文摘录如下：
        /// <code>
        ///             foreach (CustomAttributeData attributeData in CustomAttributeData.GetCustomAttributes(providerType))
        ///        {
        ///            if (attributeData.AttributeType.FullName == AliasAttibuteTypeFullName)
        ///            {
        ///                foreach (CustomAttributeTypedArgument arg in attributeData.ConstructorArguments)
        ///                {
        ///                    Debug.Assert(arg.ArgumentType == typeof(string));
        ///
        ///                    return arg.Value?.ToString();
        ///                }
        ///            }
        ///        }
        ///
        ///    return null;
        /// </code>
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection UseBoysheOLoggerFactory(this IServiceCollection services,Func<Type, string> GetAliasFunc)
        {
            if (GetAliasFunc == null) throw new ArgumentNullException(nameof(GetAliasFunc));
            ProviderAliasUtilities.GetAliasFunc = GetAliasFunc;
            services.Replace(ServiceDescriptor
                .Singleton<ILoggerFactory, BoysheO.Extensions.Logging.LoggerFactory>());
            return services;
        }
        
        /// <summary>
        /// Adds logging services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configure">The <see cref="ILoggingBuilder"/> configuration delegate.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddLoggingUsingBoysheOLoggerFactory(this IServiceCollection services, Action<ILoggingBuilder> configure,Func<Type, string> GetAliasFunc)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (GetAliasFunc == null) throw new ArgumentNullException(nameof(GetAliasFunc));
            ProviderAliasUtilities.GetAliasFunc = GetAliasFunc;
            
            services.AddOptions();
        
            services.TryAdd(ServiceDescriptor.Singleton<ILoggerFactory,BoysheO.Extensions.Logging.LoggerFactory>());
            services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));
        
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<LoggerFilterOptions>>(
                new BoysheO.Extensions.Logging.DefaultLoggerLevelConfigureOptions(LogLevel.Information)));
        
            configure(new LoggingBuilder(services));
            return services;
        }
    }
}