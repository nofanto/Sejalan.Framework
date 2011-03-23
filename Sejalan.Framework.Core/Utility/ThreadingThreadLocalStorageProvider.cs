// 
//  ThreadingThreadLocalStorageProvider.cs
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
using System.Collections.Generic;
using System.Collections;
namespace Sejalan.Framework.Utility
{
	public class ThreadingThreadLocalStorageProvider : IThreadLocalStorageProvider
	{
		private string namedDataSlot = "ThreadingThreadLocalStorage";
		
		public ThreadingThreadLocalStorageProvider ()
		{
			Thread.AllocateNamedDataSlot(namedDataSlot);
			LocalDataStoreSlot threadData = Thread.GetNamedDataSlot(namedDataSlot);
			Thread.SetData(threadData, new Dictionary<string,object>());
		}
	

		#region IThreadLocalStorageProvider implementation
		public void SaveItem (string key, object value)
		{
			LocalDataStoreSlot threadData = Thread.GetNamedDataSlot(namedDataSlot);
			Dictionary<string,object> threadDataValue= (Dictionary<string,object>) Thread.GetData(threadData);
			
			if (threadDataValue.ContainsKey(key)) {
				threadDataValue[key] = value;
			}
			else
			{
				threadDataValue.Add(key,value);
			}
			
			Thread.SetData(threadData, threadDataValue);

		}

		public object GetItem (string key)
		{
			LocalDataStoreSlot threadData = Thread.GetNamedDataSlot(namedDataSlot);
			Dictionary<string,object> threadDataValue= (Dictionary<string,object>) Thread.GetData(threadData);
			return threadDataValue[key];

		}

		public bool ContainsItem (string key)
		{
			LocalDataStoreSlot threadData = Thread.GetNamedDataSlot(namedDataSlot);
			Dictionary<string,object> threadDataValue= (Dictionary<string,object>) Thread.GetData(threadData);
			return threadDataValue.ContainsKey(key);
			
		}

		public IDictionary Items {
			get {
				LocalDataStoreSlot threadData = Thread.GetNamedDataSlot(namedDataSlot);
				Dictionary<string,object> threadDataValue= (Dictionary<string,object>) Thread.GetData(threadData);
				return  threadDataValue;
			
			}
		}
		#endregion
}
}

