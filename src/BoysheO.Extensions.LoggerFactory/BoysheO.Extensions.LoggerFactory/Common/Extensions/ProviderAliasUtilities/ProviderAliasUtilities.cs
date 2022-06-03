// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Reflection;

namespace BoysheO.Extensions.Logging
{
    public static class ProviderAliasUtilities
    {
        public const string AliasAttibuteTypeFullName = "Microsoft.Extensions.Logging.ProviderAliasAttribute";

        /// <summary>
        /// 由于IL2Cpp不实现CustomAttributeData.GetCustomAttributes，所以需要另外提供这个函数的实现
        /// 如果不需要支持ProviderAliasAttribute别名属性，可以直接返回null
        /// 原文摘录如下：
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
        public static Func<Type, string> GetAliasFunc;

        internal static string GetAlias(Type providerType)
        {
            return GetAliasFunc(providerType);
            // foreach (CustomAttributeData attributeData in CustomAttributeData.GetCustomAttributes(providerType))
            // {
            //     if (attributeData.AttributeType.FullName == AliasAttibuteTypeFullName)
            //     {
            //         foreach (CustomAttributeTypedArgument arg in attributeData.ConstructorArguments)
            //         {
            //             Debug.Assert(arg.ArgumentType == typeof(string));
            //
            //             return arg.Value?.ToString();
            //         }
            //     }
            // }
            //
            // return null;
        }
    }
}