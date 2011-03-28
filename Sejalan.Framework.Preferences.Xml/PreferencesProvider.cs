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
			result.ID = item.Attributes [idAttributeName].Value;
			result.Name = item.Attributes [nameAttributeName].Value;
			result.Value = item.Attributes [valueAttributeName].Value;
			result.Description = item.InnerText;

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

		public override void SavePreference (PreferenceItem preference)
		{
			var root = CurrentXmlPreferences.DocumentElement;
			//Get collection of preference.
			var targetCollection = root.SelectSingleNode (String.Format ("preferenceCollection[@name='{0}']", preference.CollectionName));

			if (targetCollection == null) {
				targetCollection = CurrentXmlPreferences.CreateNode (XmlNodeType.Element, "preference", null);

				var nameAttr = CurrentXmlPreferences.CreateAttribute (nameAttributeName);
				nameAttr.Value = preference.CollectionName;

				targetCollection.Attributes.Append (nameAttr);

				CurrentXmlPreferences.AppendChild (targetCollection);
			} 
			
			//TODO: update preference if any and reset cache
			
			
			CurrentXmlPreferences.Save (XmlFileName);
		}

		public PreferencesProvider ()
		{
		}
		
		public override PreferenceItemCollection GetPreferences (string collectionName)
		{
			//TODO: implement get preference using cache
			throw new System.NotImplementedException();
		}
		
		
		public override void RemovePreference (PreferenceItem preference)
		{
			//TODO: implement remove preference and reset cache
			throw new System.NotImplementedException();
		}
		
	}
}

