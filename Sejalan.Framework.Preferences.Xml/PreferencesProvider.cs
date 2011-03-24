// 
//  PreferencesProvider.cs
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
using System.Xml;

namespace Sejalan.Framework.Preferences.Xml
{
	public class PreferencesProvider : PreferencesProviderBase
	{
		private XmlDocument _CurrentXmlPreferences = null;
		internal const string xmlFileNameParameterName = "xmlFileName";
		internal const string idAttributeName = "id";
		internal const string valueAttributeName = "value";
		internal const string nameAttributeName = "name";

		protected XmlDocument CurrentXmlPreferences {
			get {
				if (_CurrentXmlPreferences == null) {
					if (String.IsNullOrEmpty (XmlFileName)) {
						throw new PreferencesExceptions ("Invalid xml file name, see configuration");
					}

					_CurrentXmlPreferences = new XmlDocument ();
					try {
						_CurrentXmlPreferences.Load (XmlFileName);
					} catch (Exception ex) {
						throw new PreferencesExceptions ("Invalid xml file name, see configuration", ex);
					}
				}

				return _CurrentXmlPreferences;

			}
		}

		public virtual string XmlFileName {
			get {
				return this.Parameters [xmlFileNameParameterName];
			}
		}

		protected virtual PreferenceItem InitializePreferenceItem (XmlNode item)
		{
			PreferenceItem result = new PreferenceItem ();
			result.PreferenceID = item.Attributes [idAttributeName].Value;
			result.PreferenceName = item.Attributes [nameAttributeName].Value;
			result.PreferenceValue = item.Attributes [valueAttributeName].Value;
			result.PreferenceDescription = item.InnerText;

			return result;
		}

		protected override PreferenceItemCollection InitializePreferences (string collectionName)
		{
			var root = CurrentXmlPreferences.DocumentElement;
			var targetCollection = root.SelectSingleNode (String.Format ("preferenceCollection[@name='{0}']", collectionName));

			if (targetCollection == null) {
				return null;
			}

			PreferenceItemCollection result = new PreferenceItemCollection (collectionName);

			foreach (XmlNode item in targetCollection.ChildNodes) {
				PreferenceItem currentPreference = InitializePreferenceItem (item);
				currentPreference.CollectionName = collectionName;
				result.Add (currentPreference);
			}
			return result;
		}

		public override void SavePreferences (PreferenceItemCollection preferences)
		{
			var root = CurrentXmlPreferences.DocumentElement;
			var targetCollection = root.SelectSingleNode (String.Format ("preferenceCollection[@name='{0}']", preferences.CollectionName));

			if (targetCollection == null) {
				targetCollection = CurrentXmlPreferences.CreateNode (XmlNodeType.Element, "preference", null);

				var nameAttr = CurrentXmlPreferences.CreateAttribute (nameAttributeName);
				nameAttr.Value = preferences.CollectionName;

				targetCollection.Attributes.Append (nameAttr);

				CurrentXmlPreferences.AppendChild (targetCollection);
			} else {
				targetCollection.RemoveAll ();
				var nameAttr = CurrentXmlPreferences.CreateAttribute (nameAttributeName);
				nameAttr.Value = preferences.CollectionName;
				targetCollection.Attributes.Append (nameAttr);
			}

			foreach (var preference in preferences) {
				XmlNode newNode = CurrentXmlPreferences.CreateNode (XmlNodeType.Element, "preference", null);

				newNode.InnerText = preference.PreferenceDescription;

				var idAttr = CurrentXmlPreferences.CreateAttribute (idAttributeName);
				idAttr.Value = preference.PreferenceID;

				var nameAttr = CurrentXmlPreferences.CreateAttribute (nameAttributeName);
				nameAttr.Value = preference.PreferenceName;

				var valueAttr = CurrentXmlPreferences.CreateAttribute (valueAttributeName);
				valueAttr.Value = preference.PreferenceValue;

				newNode.Attributes.Append (idAttr);
				newNode.Attributes.Append (nameAttr);
				newNode.Attributes.Append (valueAttr);

				targetCollection.AppendChild (newNode);
			}

			CurrentXmlPreferences.Save (XmlFileName);
		}

		public PreferencesProvider ()
		{
		}
	}
}

