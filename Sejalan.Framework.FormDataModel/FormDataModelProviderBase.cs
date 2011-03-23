// 
//  FormDataModelProviderBase.cs
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
using Sejalan.Framework.LookupDataModel;
using Sejalan.Framework.Cache;
using System.Collections.Generic;
using System.Threading;

namespace Sejalan.Framework.FormDataModel
{
	public abstract class FormDataModelProviderBase: ProviderBase
	{
		private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
		
		public FormDataModelProviderBase ()
		{
		}
		
		public LookupDataModelProviderBase LookupDataModelProvider {
			get{
				return ProviderFactory.GetInstance<LookupDataModelFactory>(ProviderRepositoryFactory.Instance.Providers[Parameters["lookupDataModelRepositoryName"]]).GetProviders<LookupDataModelProviderBase>()[Parameters["lookupDataModelProviderName"]];
			}
		}
		
		
		public virtual Form GetForm(string formName)
		{
			cacheLock.EnterUpgradeableReadLock();
			try {
				Dictionary<string, Form> cachedItems = new Dictionary<string, Form>();
				if (CacheProvider.Contains(this.Name)) {
					cachedItems = CacheProvider.Read(this.Name) as Dictionary<string,Form>;
				}
				
				if (cachedItems.ContainsKey(formName)) {
					return cachedItems[formName];
				}
				else
				{
					cacheLock.EnterWriteLock();
					try {
						Form result = InitializeForm(formName);
						
						cachedItems.Add(formName, result);
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
		
		protected abstract Form InitializeForm (string formName);
		
		public abstract void SaveForm(Form form);
		
		
		public virtual LookupItemCollection DataTypes
		{
			get{
				return this.LookupDataModelProvider.GetLookups(this.Parameters["dataTypeLookupCollectionName"]);
			}
		}
		
		public CacheProviderBase CacheProvider
		{
			get{
				return ProviderFactory.GetInstance<CacheFactory>(ProviderRepositoryFactory.Instance.Providers[Parameters["cacheProviderRepositoryName"]]).GetProviders<CacheProviderBase>()[Parameters["cacheProviderName"]];
			}
		}
	}
}

