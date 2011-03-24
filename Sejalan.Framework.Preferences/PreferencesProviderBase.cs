// 
//  PreferencesProviderBase.cs
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
using System.Threading;
using Sejalan.Framework.Cache;
using System.Collections.Generic;

namespace Sejalan.Framework.Preferences
{
	public abstract class PreferencesProviderBase: ProviderBase
	{
		private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim ();

		public PreferencesProviderBase ()
		{
		}

		public virtual PreferenceItemCollection GetPreferences (string collectionName)
		{
			cacheLock.EnterUpgradeableReadLock ();
			try {
				Dictionary<string, PreferenceItemCollection> cachedItems = new Dictionary<string, PreferenceItemCollection> ();
				if (CacheProvider.Contains (this.Name)) {
					cachedItems = CacheProvider.Read (this.Name) as Dictionary<string,PreferenceItemCollection>;
				}

				if (cachedItems.ContainsKey (collectionName)) {
					return cachedItems [collectionName];
				} else {
					cacheLock.EnterWriteLock ();
					try {
						PreferenceItemCollection result = InitializePreferences (collectionName);

						cachedItems.Add (collectionName, result);
						CacheProvider.Update (this.Name, cachedItems);
						return result;

					} finally {
						cacheLock.ExitWriteLock ();
					}
				}
			} finally {
				cacheLock.ExitUpgradeableReadLock ();
			}
		}

		protected abstract PreferenceItemCollection InitializePreferences (string collectionName);

		public abstract void SavePreferences (PreferenceItemCollection lookups);

		public CacheProviderBase CacheProvider {
			get {
				return ProviderFactory.GetInstance<CacheFactory> (ProviderRepositoryFactory.Instance.Providers [Parameters ["cacheProviderRepositoryName"]]).GetProviders<CacheProviderBase> () [Parameters ["cacheProviderName"]];
			}
		}

	}
}



