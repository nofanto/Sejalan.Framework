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
using Sejalan.Framework.Utility;
using Sejalan.Framework.Provider;
using System.Xml;

namespace Sejalan.Framework.Preferences.Xml.Test
{
	[TestFixture()]
	public class Test
	{
		[TestFixtureSetUp()]
		public void SetUp ()
		{
			StorageProviderFactory.Current = new ThreadLocalStorageProvider ();
		}

		[Test()]
		public void CanGetPreferencesConfiguration ()
		{
			PreferencesProvider preferences = ProviderFactory.GetInstance<PreferencesFactory> (ProviderRepositoryFactory.Instance.Provider).GetDefaultProvider<PreferencesProvider> ();

			Assert.AreEqual (preferences.Name, "PreferencesTest");
			Assert.AreEqual (preferences.XmlFileName, "Preferences.xml");

		}

		[Test()]
		public void CanReadAddRemovePreferenceItem ()
		{
			PreferencesProvider preferences = ProviderFactory.GetInstance<PreferencesFactory> (ProviderRepositoryFactory.Instance.Provider).GetDefaultProvider<PreferencesProvider> ();

			Assert.AreNotEqual (preferences.GetPreferences ("appSetting").Count, 0);
			Assert.AreEqual (preferences.GetPreferences ("appSetting").Count, 2);
			Assert.AreEqual (preferences.GetPreferences ("appSetting") ["1"].ID, "1");
			Assert.AreEqual (preferences.GetPreferences ("appSetting") ["1"].Description, "preference no 1");

			PreferenceItemCollection appSetting = preferences.GetPreferences ("appSetting");
			PreferenceItem newItem = new PreferenceItem ();
			newItem.ID = "3";
			newItem.Description = "Percentage";
			newItem.Name = "3";
			newItem.Value = "3";

			appSetting.Add (newItem);

			preferences.SavePreferences (appSetting);

			var xmlDoc = new XmlDocument ();
			xmlDoc.Load (preferences.XmlFileName);

			var dataTypeNode = xmlDoc.DocumentElement.SelectSingleNode (String.Format ("preferenceCollection[@name='{0}']", appSetting.CollectionName));
			Assert.AreEqual (dataTypeNode.ChildNodes.Count, 3);
			Assert.AreEqual (dataTypeNode.ChildNodes [2].Attributes ["name"].Value, "3");

			appSetting = preferences.GetPreferences("appSetting");
			appSetting.Remove (appSetting ["3"]);
			preferences.SavePreferences (appSetting);

			xmlDoc = new XmlDocument ();
			xmlDoc.Load (preferences.XmlFileName);

			dataTypeNode = xmlDoc.DocumentElement.SelectSingleNode (String.Format ("preferenceCollection[@name='{0}']", appSetting.CollectionName));
			Assert.AreEqual (dataTypeNode.ChildNodes.Count, 2);
			Assert.AreNotEqual (dataTypeNode.ChildNodes [1].Attributes ["name"].Value, "3");


		}


	}
}

