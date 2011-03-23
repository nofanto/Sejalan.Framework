// 
//  Helper.cs
//  
//  Author:
//       nofanto ibrahim <nofanto.ibrahim@gmail.com>
// 
//  Copyright (c) 2011 sejalan
// 
//  This library is free software; you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as
//  published by the Free Software Foundation; either version 2.1 of the
//  License, or (at your option) any later version.
// 
//  This library is distributed in the hope that it will be useful, but
//  WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;
using System.Threading;
using System.Reflection;
using System.Reflection.Emit;

namespace Sejalan.Framework.Utility
{
    public delegate void GenericSetter(object target, object value);
    public delegate object GenericGetter(object target);

    /// <summary>
    /// Helper class for Sejalan.Library
    /// </summary>
    public class Helper
	{

        /// <summary>
        /// Retries the specified work.
        /// </summary>
        /// <param name="maximumRetries">The maximum retries.</param>
        /// <param name="pauseBetweenRetry">The pause between retry.</param>
        /// <param name="workToRetry">The work to retry.</param>
        public static void Retry(int maximumRetries, int pauseBetweenRetry, Action workToRetry)
        {
            if (workToRetry == null)
                throw new ArgumentNullException("workToRetry", "workToRetry is null.");

            Retry(maximumRetries, pauseBetweenRetry, convertActionToFunc(workToRetry));
        }

        /// <summary>
        /// Retries the specified work.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="maximumRetries">The maximum retries.</param>
        /// <param name="pauseBetweenRetry">The pause between retry.</param>
        /// <param name="workToRetry">The work to retry.</param>
        /// <returns></returns>
        public static T Retry<T>(int maximumRetries, int pauseBetweenRetry, Func<T> workToRetry)
        {
            if (workToRetry == null)
                throw new ArgumentNullException("workToRetry", "workToRetry is null.");

            int retryCounter = 0;
            while (true)
            {
                try
                {
                    return workToRetry();
                }
                catch (Exception)
                {
                    retryCounter++;
                    Thread.Sleep(pauseBetweenRetry);
                    if (retryCounter > maximumRetries)
                    {
                        throw;
                    }
                }
            }
        }

        private static Func<object> convertActionToFunc(Action action)
        {
            return delegate { action(); return null; };
        }

        /// <summary>
        /// Creates the set method.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns></returns>
        public static GenericSetter CreateSetMethod(PropertyInfo propertyInfo)
        {
            MethodInfo setMethod = propertyInfo.GetSetMethod();
            if (setMethod == null)
            {
                return null;
            }

            Type[] arguments = new Type[2];
            arguments[0] = typeof(object);
            arguments[1] = typeof(object);

            DynamicMethod setter = new DynamicMethod(String.Concat("_Set", propertyInfo.Name, "_"), typeof(void), arguments, propertyInfo.DeclaringType);
            ILGenerator generator = setter.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
            generator.Emit(OpCodes.Ldarg_1);

            if (propertyInfo.PropertyType.IsClass)
            {
                generator.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
            }
            else
            {
                generator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
            }

            generator.EmitCall(OpCodes.Callvirt, setMethod, null);
            generator.Emit(OpCodes.Ret);

            return setter.CreateDelegate(typeof(GenericSetter)) as GenericSetter;
        }

        /// <summary>
        /// Creates the get method.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns></returns>
        public static GenericGetter CreateGetMethod(PropertyInfo propertyInfo)
        {
            MethodInfo getMethod = propertyInfo.GetGetMethod();
            if (getMethod == null)
            {
                return null;
            }

            Type[] arguments = new Type[1];
            arguments[0] = typeof(object);

            DynamicMethod getter = new DynamicMethod(String.Concat("_Get", propertyInfo.Name, "_"), typeof(object), arguments, propertyInfo.DeclaringType);
            
            ILGenerator generator = getter.GetILGenerator();
            generator.DeclareLocal(typeof(object));
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
            generator.EmitCall(OpCodes.Callvirt, getMethod, null);

            if (!propertyInfo.PropertyType.IsClass)
            {
                generator.Emit(OpCodes.Box, propertyInfo.PropertyType);
            }

            generator.Emit(OpCodes.Ret);

            return getter.CreateDelegate(typeof(GenericGetter)) as GenericGetter;
        }

	 }
}
