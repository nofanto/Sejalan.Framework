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

using System;
using NUnit.Framework;
using Sejalan.Framework.Provider;
using Sejalan.Framework.Provider.AppConfig;
using Sejalan.Framework.Utility;
using System.Xml;
namespace Sejalan.Framework.LookupDataModel.Xml.Test
{
	[TestFixture()]
	public class Test
	{
		[TestFixtureSetUp()]
		public void SetUp()
		{
			ThreadLocalStorageProviderFactory.Current = new ThreadingThreadLocalStorageProvider();
		}
		
		[Test()]
		public void CanGetLookupDataModelConfiguration ()
		{
			LookupDataModelProvider lookupDataModel = ProviderFactory.GetInstance<LookupDataModelFactory> (ProviderRepositoryFactory.Instance.Provider).GetDefaultProvider<LookupDataModelProvider> ();
			
			Assert.AreEqual (lookupDataModel.Name, "LookupDataModelTest");
			Assert.AreEqual (lookupDataModel.XmlFileName, "LookupDataModel.xml");

		}
		
		[Test()]
		public void CanReadAddRemoveLookupItem ()
		{
			LookupDataModelProvider lookupDataModel = ProviderFactory.GetInstance<LookupDataModelFactory> (ProviderRepositoryFactory.Instance.Provider).GetDefaultProvider<LookupDataModelProvider> ();
			
			Assert.AreNotEqual (lookupDataModel.GetLookups ("dataType").Count, 0);
			Assert.AreEqual (lookupDataModel.GetLookups ("dataType").Count, 2);
			Assert.AreEqual (lookupDataModel.GetLookups ("dataType")["1"].LookupID, "1");
			Assert.AreEqual (lookupDataModel.GetLookups ("dataType")["1"].LookupDesc, "String");

			LookupItemCollection dataTypes = lookupDataModel.GetLookups ("dataType");
			LookupItem newItem = new LookupItem ();
			newItem.LookupID = "3";
			newItem.LookupDesc = "Percentage";
			newItem.LookupCode = "3";
			
			dataTypes.Add (newItem);
			
			lookupDataModel.SaveLookups (dataTypes);
			
			var xmlDoc = new XmlDocument ();
			xmlDoc.Load (lookupDataModel.XmlFileName);
		
			var dataTypeNode = xmlDoc.DocumentElement.SelectSingleNode (String.Format ("lookupCollection[@name='{0}']", dataTypes.CollectionName));
			Assert.AreEqual (dataTypeNode.ChildNodes.Count, 3);
			Assert.AreEqual (dataTypeNode.ChildNodes[2].Attributes["code"].Value, "3");
		
			dataTypes = lookupDataModel.GetLookups ("dataType");
			dataTypes.Remove (dataTypes["3"]);
			lookupDataModel.SaveLookups (dataTypes);
			
			xmlDoc = new XmlDocument ();
			xmlDoc.Load (lookupDataModel.XmlFileName);
			
			dataTypeNode = xmlDoc.DocumentElement.SelectSingleNode (String.Format ("lookupCollection[@name='{0}']", dataTypes.CollectionName));
			Assert.AreEqual (dataTypeNode.ChildNodes.Count, 2);
			Assert.AreNotEqual (dataTypeNode.ChildNodes[1].Attributes["code"].Value, "3");
		
			
		}
		
	}
}

