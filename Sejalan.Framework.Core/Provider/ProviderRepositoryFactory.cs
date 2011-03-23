// 
//  ProviderRepositoryFactory.cs
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
using Sejalan.Framework.Collection;
using Sejalan.Framework.Provider.AppConfig;
using Sejalan.Framework.Utility;
using System.Reflection;
using System.Collections.Specialized;

namespace Sejalan.Framework.Provider
{
	public sealed class ProviderRepositoryFactory
	{
		private ReaderWriterLockSlim m_CacheLock = new ReaderWriterLockSlim();
        private Dictionary<string, object> _Cache;
		
		private static ReaderWriterLockSlim m_StaticCacheLock = new ReaderWriterLockSlim();
		private IList<ProviderSettings> providerSettings = null;
		private string defaultProviderName = String.Empty;
		private static string providerRepositoryFactory = "providerRepositoryFactory";
		
    
		private ProviderRepositoryFactory ()
		{
			this._Cache = new Dictionary<string,object>();
			ProviderRepository repository = new ProviderRepository();
			ProviderRepositoryInfo info = repository.GetProviderRepositoryInfo(Key);
			providerSettings = info.ProviderSettings;
			defaultProviderName = info.DefaultProviderName;

		}
		
		private string Key {
			get{ return "sejalan.framework/providerRepository";}
			
		}
		
		private Dictionary<string, object> Cache
        {
            get
            {
                return this._Cache;
            }
        }
		
		public static ProviderRepositoryFactory Instance
		{
			get{
				m_StaticCacheLock.EnterUpgradeableReadLock();
				try 
				{
					if( ThreadLocalStorageProviderFactory.Current.ContainsItem(providerRepositoryFactory))
					{
						return (ProviderRepositoryFactory) ThreadLocalStorageProviderFactory.Current.GetItem(providerRepositoryFactory);
					}
					else
					{
						m_StaticCacheLock.EnterWriteLock();
						try {
							ConstructorInfo constructor = null;
	
							try
							{
								// Binding flags exclude public constructors.
								constructor = typeof(ProviderRepositoryFactory).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[0], null);
							}
							catch (Exception exception)
							{
								throw new CoreException("Error on initialize provider repository factory", exception);
							}
		
							if (constructor == null || constructor.IsAssembly) // Also exclude internal constructors.
								throw new CoreException(string.Format("A private or protected constructor is missing for '{0}'.", "ProviderRepositoryFactory"));
				
							ProviderRepositoryFactory instance = (ProviderRepositoryFactory) constructor.Invoke(null);
							ThreadLocalStorageProviderFactory.Current.SaveItem(providerRepositoryFactory,instance);
							return instance;
	
						} 
						finally 
						{
							m_StaticCacheLock.ExitWriteLock();
						}

										}
				} 
				finally 
				{
					m_StaticCacheLock.ExitUpgradeableReadLock();
				}
				
			}
		}
		
		public ProviderRepositoryBase Provider
        {
            get
            {
                ProviderRepositoryBase result = Providers[defaultProviderName];
                if(result == null)
                    throw new CoreException(String.Format(System.Globalization.CultureInfo.InvariantCulture, "ProviderNoTypeName", defaultProviderName));
                return result;
            }
        }
		
		public ReadOnlyDictionary<string,ProviderRepositoryBase> Providers
        {
			get
			{
	            m_CacheLock.EnterUpgradeableReadLock();
	
	            try
	            {
	                if (Cache.ContainsKey(this.Key))
	                {
	                    return new ReadOnlyDictionary<string, ProviderRepositoryBase>((Dictionary<string,ProviderRepositoryBase>)Cache[this.Key]);
	                }
	                else
	                {
	                    m_CacheLock.EnterWriteLock();
	                    try
	                    {
	                        var providerCollection = new Dictionary<string, ProviderRepositoryBase>();
	
	                        if (providerSettings.Count == 0)
	                            throw new CoreException("NoProviderDefined");
	
	                        foreach (ProviderSettings providerSetting in providerSettings)
	                        {
	                            string typeName = providerSetting.Type;
	                            Type provider = Type.GetType(typeName);
	                            if (provider == null)
	                                throw new CoreException(String.Format(System.Globalization.CultureInfo.InvariantCulture, "ProviderLoadError", providerSetting.Name));
	                            ProviderRepositoryBase currentProvider = this.InstantiateProvider(providerSetting, provider);
	                            providerCollection.Add(currentProvider.Name, currentProvider as ProviderRepositoryBase);
	                        }
	                        Cache.Add(this.Key, providerCollection);
	
	                        return new ReadOnlyDictionary<string, ProviderRepositoryBase>(providerCollection);
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
        }

        private ProviderRepositoryBase InstantiateProvider(ProviderSettings providerSettings, Type providerType)
        {
            if (providerSettings == null) throw new ArgumentNullException("providerSettings");
            if (providerType == null) throw new ArgumentNullException("providerType");

            ProviderRepositoryBase providerBase = null;
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

                providerBase = (ProviderRepositoryBase)Activator.CreateInstance(providerBaseType);

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

