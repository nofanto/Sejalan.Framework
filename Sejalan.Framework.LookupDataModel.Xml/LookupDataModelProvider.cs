// 
//  LookupDataModelProvider.cs
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
namespace Sejalan.Framework.LookupDataModel.Xml
{
	public class LookupDataModelProvider : LookupDataModelProviderBase
	{
		private XmlDocument _CurrentXmlLookup = null;
		internal const string xmlFileNameParameterName = "xmlFileName";
		internal const string idAttributeName = "id";
		internal const string codeAttributeName = "code";
		internal const string viewOrderAttributeName = "viewOrder";
		internal const string nameAttributeName = "name";

		protected XmlDocument CurrentXmlLookup {
			get{
				if (_CurrentXmlLookup == null) {
					if (String.IsNullOrEmpty(XmlFileName)) {
						throw new LookupDataModelExceptions("Invalid xml file name, see configuration");
					}
					
					_CurrentXmlLookup = new XmlDocument();
					try {
						_CurrentXmlLookup.Load(XmlFileName);
					} catch (Exception ex) {
						throw new LookupDataModelExceptions("Invalid xml file name, see configuration", ex);
					}
				}
				
				return _CurrentXmlLookup;

			}
		}
		
		
		public virtual string XmlFileName {
			get{
				return this.Parameters[xmlFileNameParameterName];
			}
		}
			
		
		protected virtual LookupItem InitializeLookupItem (XmlNode item)
		{
			LookupItem result = new LookupItem();
			result.LookupID = item.Attributes[idAttributeName].Value;
			result.LookupCode = item.Attributes[codeAttributeName].Value;
			result.ViewOrder = Int32.Parse(item.Attributes[viewOrderAttributeName].Value);
			result.LookupDesc = item.InnerText;
			
			return result;
		}

		protected override LookupItemCollection InitializeLookups (string collectionName)
		{
			var root = CurrentXmlLookup.DocumentElement;
			var targetCollection = root.SelectSingleNode(String.Format("lookupCollection[@name='{0}']",collectionName));
	
			if (targetCollection == null) {
				return null;
			}
	
			LookupItemCollection result = new LookupItemCollection(collectionName);
			
			foreach (XmlNode item in targetCollection.ChildNodes) {
				LookupItem currentLookup = InitializeLookupItem(item);
				currentLookup.CollectionName = collectionName;
				result.Add(currentLookup);
			}
			return result;
		}
		
		
		
		public override void SaveLookups (LookupItemCollection lookups)
		{
			var root = CurrentXmlLookup.DocumentElement;
			var targetCollection = root.SelectSingleNode (String.Format ("lookupCollection[@name='{0}']", lookups.CollectionName));
	
			if (targetCollection == null) {
				targetCollection = CurrentXmlLookup.CreateNode (XmlNodeType.Element, "lookup", null);
				
				var nameAttr = CurrentXmlLookup.CreateAttribute (nameAttributeName);
				nameAttr.Value = lookups.CollectionName;
				
				targetCollection.Attributes.Append (nameAttr);
				
				CurrentXmlLookup.AppendChild (targetCollection);
			}
			else
			{
				targetCollection.RemoveAll ();
				var nameAttr = CurrentXmlLookup.CreateAttribute (nameAttributeName);
				nameAttr.Value = lookups.CollectionName;
				targetCollection.Attributes.Append (nameAttr);
			}
			
			foreach (var lookup in lookups) {
				XmlNode newNode = CurrentXmlLookup.CreateNode(XmlNodeType.Element,"lookup", null);
				
				newNode.InnerText = lookup.LookupDesc;
				
				var idAttr = CurrentXmlLookup.CreateAttribute(idAttributeName);
				idAttr.Value = lookup.LookupID;
				
				var codeAttr = CurrentXmlLookup.CreateAttribute(codeAttributeName);
				codeAttr.Value = lookup.LookupCode;
				
				var viewOrderAttr = CurrentXmlLookup.CreateAttribute(viewOrderAttributeName);
				viewOrderAttr.Value = lookup.ViewOrder.ToString();
				
				newNode.Attributes.Append(idAttr);
				newNode.Attributes.Append(codeAttr);
				newNode.Attributes.Append(viewOrderAttr);
				
				targetCollection.AppendChild(newNode);
			}
			
			CurrentXmlLookup.Save(XmlFileName);
		}
		
		public LookupDataModelProvider ()
		{
		}
	}
}

