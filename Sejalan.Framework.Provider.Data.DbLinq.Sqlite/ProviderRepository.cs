// 
//  ProviderRepository.cs
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
using Sejalan.Framework.Data.DbLinq;
using System.Linq;
using System.Collections.Specialized;

namespace Sejalan.Framework.Provider.Data.DbLinq.Sqlite
{
	public class ProviderRepository : ProviderRepositoryBase
	{
		#region implemented abstract members of Sejalan.Framework.Provider.ProviderRepositoryBase
		public override ProviderRepositoryInfo GetProviderRepositoryInfo (string providerFactoryKey)
		{
			if (String.IsNullOrEmpty(providerFactoryKey)) {
				throw new ArgumentNullException("providerFactoryKey");
			}
			
			ProviderRepositoryInfo result = new ProviderRepositoryInfo();
			result.DefaultProviderName = GetDefaultProviderName(providerFactoryKey);
			foreach (ProviderSettings item in GetProviders(providerFactoryKey).Values) {
				result.ProviderSettings.Add(item);
			}
			
			return result;
		}
		
		private Dictionary<string, ProviderSettings> GetProviders(string providerKey)
		{
			var result = new Dictionary<String, ProviderSettings>();
			using (ProviderDataContext dc = ProviderFactory.GetInstance<DataContextFactory>(ProviderRepositoryFactory.Instance.Providers[this.Parameters["dataContextProviderRepositoryName"]]).GetProviders<DataContextProviderBase>()[this.Parameters["dataContextProviderName"]].DataContext as ProviderDataContext)
			{
				foreach (ProviderValueObject provider in dc.Providers.Where(p => p.Key == providerKey).ToList()) {
					var newProviderSettings = new ProviderSettings(provider.Name,provider.Type);
					foreach (ProviderParameterValueObject parameter in dc.ProviderParameters.Where(p => p.FKProviders == provider.Id).ToList()) {
						newProviderSettings.Parameters.Add(parameter.Name, parameter.Value);
					}
					result.Add(newProviderSettings.Name, newProviderSettings);
				}
			}
			
			return result;
		}
		
		private string GetDefaultProviderName(string providerKey)
		{
			string providerName = string.Empty;
			
			using (ProviderDataContext dc = ProviderFactory.GetInstance<DataContextFactory>(ProviderRepositoryFactory.Instance.Providers[this.Parameters["dataContextProviderRepositoryName"]]).GetProviders<DataContextProviderBase>()[this.Parameters["dataContextProviderName"]].DataContext as ProviderDataContext)
			{
				ProviderValueObject result = dc.Providers.Where(p => p.IsDefault).SingleOrDefault();
                if (result != null)
                {
                    providerName = result.Name;
                }
			}

            return providerName;
		}
		
		public override void SaveProviderRepositoryInfo (string providerFactoryKey, ProviderRepositoryInfo providerRepositoryInfo)
		{
			using (ProviderDataContext dc = ProviderFactory.GetInstance<DataContextFactory>(ProviderRepositoryFactory.Instance.Providers[this.Parameters["dataContextProviderRepositoryName"]]).GetProviders<DataContextProviderBase>()[this.Parameters["dataContextProviderName"]].DataContext as ProviderDataContext)
			{
                dc.ExecuteCommand(String.Format("DELETE FROM ProviderParameters WHERE FKProviders IN (SELECT Id FROM Providers WHERE Key = '{0}')", providerFactoryKey));
                dc.ExecuteCommand(String.Format("DELETE FROM Providers WHERE Key = '{0}'", providerFactoryKey));
				
				foreach (ProviderSettings provider in providerRepositoryInfo.ProviderSettings) {
					
					var newProvider = new ProviderValueObject();
					newProvider.Key = providerFactoryKey;
					newProvider.Name = provider.Name;
					newProvider.Type = provider.Type;
					newProvider.IsDefault = (providerRepositoryInfo.DefaultProviderName == provider.Name);
					
					dc.Providers.InsertOnSubmit(newProvider);
                    dc.SubmitChanges();

					foreach (var item in provider.Parameters.Keys) {
						if (item.ToString().ToUpper() != "NAME" && item.ToString().ToUpper() != "TYPE") {
							var newParameter = new ProviderParameterValueObject();
							newParameter.FKProviders = newProvider.Id;
							newParameter.Name = item.ToString();
							newParameter.Value = provider.Parameters[item.ToString()];
							
							dc.ProviderParameters.InsertOnSubmit(newParameter);
						}
						
					}
                    dc.SubmitChanges();
				}
			}
		}
		
		#endregion
		public ProviderRepository ()
		{
		}
	}
}

