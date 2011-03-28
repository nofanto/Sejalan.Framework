// 
//  Test.cs
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

using NUnit.Framework;
using System;
using Sejalan.Framework.Utility;
using Sejalan.Framework.LookupDataModel.Xml;
using Sejalan.Framework.LookupDataModel;
namespace Sejalan.Framework.Provider.Data.DbLinq.Sqlite.Test
{
	[TestFixture()]
	public class Test
	{
		[TestFixtureSetUp()]
		public void SetUp()
		{
			StorageProviderFactory.Current = new ThreadLocalStorageProvider();
			
			//get lookup configuration from app config.
			LookupDataModelFactory lookupFactory = ProviderFactory.GetInstance<LookupDataModelFactory>(ProviderRepositoryFactory.Instance.Providers["AppConfig"]);
			
			//save it to database
			lookupFactory.SaveInstance(ProviderRepositoryFactory.Instance.Providers["SqliteConfig"]);
		}
		
		[Test()]
		public void CanGetLookupDataModelConfigurationFromDatabase ()
		{
			LookupDataModelFactory lookupFactory = ProviderFactory.GetInstance<LookupDataModelFactory>(ProviderRepositoryFactory.Instance.Providers["SqliteConfig"]);
			
			LookupDataModelProvider lookupDataModel = lookupFactory.GetDefaultProvider<LookupDataModelProvider>();
			
			Assert.AreEqual(lookupDataModel.Name, "LookupDataModelTest");
			Assert.AreEqual(lookupDataModel.XmlFileName, "LookupDataModel.xml");

		}
		
		[Test()]
		public void CanReadLookup()
		{
			LookupDataModelProvider lookupDataModel = ProviderFactory.GetInstance<LookupDataModelFactory>(ProviderRepositoryFactory.Instance.Providers["SqliteConfig"]).GetDefaultProvider<LookupDataModelProvider>();
			
			Assert.AreNotEqual(lookupDataModel.GetLookups("dataType").Count,0);
			Assert.AreEqual(lookupDataModel.GetLookups("dataType").Count,2);
			Assert.AreEqual(lookupDataModel.GetLookups("dataType")[0].LookupID,"1");
			Assert.AreEqual(lookupDataModel.GetLookups("dataType")[0].LookupDesc,"String");
		}

	}
}

