// 
//  DynamicFieldGetterSetter.cs
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
using System.Collections.Generic;
using System.Reflection;

namespace Sejalan.Framework.Utility
{


	public class DynamicFieldGetterSetter
	{
		private Dictionary<string,GenericGetter> _genericGetters = new Dictionary<string, GenericGetter>();
		private Dictionary<string,GenericSetter> _genericSetters = new Dictionary<string, GenericSetter>();

		public DynamicFieldGetterSetter (Type objectType)
		{
			foreach (PropertyInfo item in objectType.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public))
            {
				_genericGetters.Add(item.Name,Helper.CreateGetMethod(item));
				_genericSetters.Add(item.Name,Helper.CreateSetMethod(item));
            }
			
		}
		
		public object GetColumnValue(string fieldName, object target)
		{
			return _genericGetters[fieldName](target);
		}
		
		public void SetColumnValue(string fieldName, object target, object value)
		{
			_genericSetters[fieldName](target,value);
		}
	}
	
	public static class Extensions
	{
		public static DynamicFieldGetterSetter GetDynamicFieldGetterSetter<T>(this T objectType) where T :Type
		{
			return new DynamicFieldGetterSetter(objectType);
		}
	}
}
