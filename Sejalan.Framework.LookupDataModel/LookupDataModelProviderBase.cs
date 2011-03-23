// 
//  LookupDataModelProviderBase.cs
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
using Sejalan.Framework.Provider;
using System.Collections.Generic;
using Sejalan.Framework.Cache;
using System.Threading;
namespace Sejalan.Framework.LookupDataModel
{
	public abstract class LookupDataModelProviderBase: ProviderBase
	{
		private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
		
		public LookupDataModelProviderBase ()
		{
		}
		
		public virtual LookupItemCollection GetLookups(string collectionName)
		{
			cacheLock.EnterUpgradeableReadLock();
			try {
				Dictionary<string, LookupItemCollection> cachedItems = new Dictionary<string, LookupItemCollection>();
				if (CacheProvider.Contains(this.Name)) {
					cachedItems = CacheProvider.Read(this.Name) as Dictionary<string,LookupItemCollection>;
				}
				
				if (cachedItems.ContainsKey(collectionName)) {
					return cachedItems[collectionName];
				}
				else
				{
					cacheLock.EnterWriteLock();
					try {
						LookupItemCollection result = InitializeLookups(collectionName);
						
						cachedItems.Add(collectionName, result);
						CacheProvider.Update(this.Name,cachedItems);
						return result;
						
					} finally  {
						cacheLock.ExitWriteLock();
					}
				}
			} finally {
				cacheLock.ExitUpgradeableReadLock();
			}
		}
		protected abstract LookupItemCollection InitializeLookups (string collectionName);
		
		public abstract void SaveLookups(LookupItemCollection lookups);
		
		public CacheProviderBase CacheProvider
		{
			get{
				return ProviderFactory.GetInstance<CacheFactory>(ProviderRepositoryFactory.Instance.Providers[Parameters["cacheProviderRepositoryName"]]).GetProviders<CacheProviderBase>()[Parameters["cacheProviderName"]];
			}
		}
		
	}
}

