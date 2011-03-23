// 
//  ProviderFactory.cs
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
using System.Threading;
using Sejalan.Framework.Collection;
using System.Collections.Specialized;
namespace Sejalan.Framework.Provider
{
	   /// <summary>
    /// Abstract class that provide base functionality of framework configuration.
    /// </summary>
    public abstract class ProviderFactory
    {
        private ReaderWriterLockSlim m_CacheLock = new ReaderWriterLockSlim();
        private Dictionary<string, object> _Cache;
		
		private static ReaderWriterLockSlim m_StaticCacheLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
		private static Dictionary<string,object> _StaticCache = new Dictionary<string, object>();
    
		
	    /// <summary>
        /// Initializes a new instance of the <see cref="ProviderFactory"/> class.
        /// </summary>
        protected ProviderFactory()
        {
            this._Cache = new Dictionary<string,object>();
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        protected abstract string Key
        {
            get;
        }

        /// <summary>
        /// Gets the cache.
        /// </summary>
        /// <value>The cache.</value>
        protected Dictionary<string, object> Cache
        {
            get
            {
                return this._Cache;
            }
        }
		
		public static S GetInstance<S>(ProviderRepositoryBase repository) where S: ProviderFactory, new()
		{
			m_StaticCacheLock.EnterUpgradeableReadLock();
			string key = String.Format("{0}^_^{1}",repository.Name, typeof(S).Name);
			try {
				if (_StaticCache.ContainsKey(key)) {
					return _StaticCache[key] as S;
				}
				else
				{
					m_StaticCacheLock.EnterWriteLock();
					try {
						
						S result = new S();
						result.Initialize(repository);
						_StaticCache.Add(key, result);
						return result;
						
					} finally {
						m_StaticCacheLock.ExitWriteLock();
					}
				}
			} finally {
				m_StaticCacheLock.ExitUpgradeableReadLock();
			}
		}

		public void SaveInstance(ProviderRepositoryBase repository)
		{
			ProviderRepositoryInfo info = new ProviderRepositoryInfo();
            foreach (var item in providerSettings)
	        {
		         info.ProviderSettings.Add(item);
	        }
			info.DefaultProviderName = defaultProviderName;
			repository.SaveProviderRepositoryInfo(this.Key,info);
		}
		
		public void Initialize(ProviderRepositoryBase repository)
		{
			ProviderRepositoryInfo info = repository.GetProviderRepositoryInfo(Key);
			providerSettings = info.ProviderSettings;
			defaultProviderName = info.DefaultProviderName;
		}
		
        public T GetDefaultProvider<T>() where T : ProviderBase
        {
            T result = GetProviders<T>()[defaultProviderName];
            if(result == null)
                throw new CoreException(String.Format(System.Globalization.CultureInfo.InvariantCulture, "ProviderNoTypeName", defaultProviderName));
            return result;
        }

		protected IList<ProviderSettings> providerSettings = null;
		
		protected string defaultProviderName = String.Empty;
		        		
		
        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <value>The providers.</value>
        public virtual ReadOnlyDictionary<string,T> GetProviders<T>() where T : ProviderBase
        {
            m_CacheLock.EnterUpgradeableReadLock();

            try
            {
                if (Cache.ContainsKey(this.Key))
                {
                    return new ReadOnlyDictionary<string, T>((Dictionary<string,T>)Cache[this.Key]);
                }
                else
                {
                    m_CacheLock.EnterWriteLock();
                    try
                    {
                        var providerCollection = new Dictionary<string, T>();

                        if (providerSettings.Count == 0)
                            throw new CoreException("NoProviderDefined");

                        foreach (ProviderSettings providerSetting in providerSettings)
                        {
                            string typeName = providerSetting.Type;
                            Type provider = Type.GetType(typeName);
                            if (provider == null)
                                throw new CoreException(String.Format(System.Globalization.CultureInfo.InvariantCulture, "ProviderLoadError", providerSetting.Name));
                            ProviderBase currentProvider = this.InstantiateProvider(providerSetting, provider);
                            providerCollection.Add(currentProvider.Name, currentProvider as T);
                        }
                        Cache.Add(this.Key, providerCollection);

                        return new ReadOnlyDictionary<string, T>(providerCollection);
                    }
                    finally
                    {
                        m_CacheLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                m_CacheLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Instantiates the provider.
        /// </summary>
        /// <param name="providerSettings">The provider settings.</param>
        /// <param name="providerType">Type of the provider.</param>
        /// <returns><see cref="ProviderBase"/></returns>
        protected virtual ProviderBase InstantiateProvider(ProviderSettings providerSettings, Type providerType)
        {
            if (providerSettings == null) throw new ArgumentNullException("providerSettings");
            if (providerType == null) throw new ArgumentNullException("providerType");

            ProviderBase providerBase = null;
            try
            {
                string providerBaseTypeName = (providerSettings.Type == null) ? null : providerSettings.Type.Trim();
                if (string.IsNullOrEmpty(providerBaseTypeName))
                {
                    throw new ArgumentException(String.Format(System.Globalization.CultureInfo.InvariantCulture, "ProviderNoTypeName", new object[] { providerBaseTypeName }));
                }
                Type providerBaseType = Type.GetType(providerBaseTypeName, true, true);

                if (!providerType.IsAssignableFrom(providerBaseType))
                {
                    throw new ArgumentException(String.Format(System.Globalization.CultureInfo.InvariantCulture, "ProviderMustImplementType", new object[] { providerType.ToString() }));
                }

                providerBase = (ProviderBase)Activator.CreateInstance(providerBaseType);

                NameValueCollection providerSettingParameters = providerSettings.Parameters;
                NameValueCollection newProviderSettingParameters = new NameValueCollection(providerSettingParameters.Count, StringComparer.Ordinal);
                foreach (string parameter in providerSettingParameters)
                {
                    newProviderSettingParameters[parameter] = providerSettingParameters[parameter];
                }
                providerBase.Initialize(providerSettings.Name, newProviderSettingParameters);
            }
            catch (Exception ex)
            {
                if (ex is CoreException)
                {
                    throw;
                }
                throw new CoreException(ex.Message);
            }
            return providerBase;
        }
    }
}

