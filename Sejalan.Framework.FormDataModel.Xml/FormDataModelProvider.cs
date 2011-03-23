// 
//  FormDataModelProvider.cs
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
namespace Sejalan.Framework.FormDataModel.Xml
{
	public class FormDataModelProvider : FormDataModelProviderBase
	{
		
		private XmlDocument _CurrentXmlForm = null;
		internal const string xmlFileNameParameterName = "xmlFileName";
		internal const string idAttributeName = "id";
		internal const string nameAttributeName = "name";
		internal const string isRequiredAttributeName = "isRequired";
		internal const string fieldLengthAttributeName = "fieldLength";
		internal const string dataTypeAttributeName = "dataType";
		
		protected XmlDocument CurrentXmlForm {
			get{
				if (_CurrentXmlForm == null) {
					if (String.IsNullOrEmpty(XmlFileName)) {
						throw new FormDataModelExceptions("Invalid xml file name, see configuration");
					}
					
					_CurrentXmlForm = new XmlDocument();
					try {
						_CurrentXmlForm.Load(XmlFileName);
					} catch (Exception ex) {
						throw new FormDataModelExceptions("Invalid xml file name, see configuration", ex);
					}
				}
				
				return _CurrentXmlForm;

			}
		}
		
		
		public virtual string XmlFileName {
			get{
				return this.Parameters[xmlFileNameParameterName];
			}
		}
			
		
		protected virtual Field IntializeField (XmlNode item)
		{
			Field result = new Field();
			result.Id = item.Attributes[idAttributeName].Value;
			result.Name = item.Attributes[nameAttributeName].Value;
			if (item.Attributes[isRequiredAttributeName] != null) {
				result.IsRequired = Convert.ToBoolean(item.Attributes[isRequiredAttributeName].Value);
			}
			else
			{
				result.IsRequired = false;	
			}
			
			result.FieldLength = Convert.ToInt32(item.Attributes[fieldLengthAttributeName].Value);
			result.DataType = DataTypes[item.Attributes[dataTypeAttributeName].Value];
			result.Caption = item.InnerText;
			
			return result;
		}

		protected virtual Form InitializeForm (XmlNode item)
		{
			Form result = new Form();
			result.Id = item.Attributes[idAttributeName].Value;
			result.Name = item.Attributes[nameAttributeName].Value;
			
			foreach (XmlNode childNode in item.ChildNodes) {
				if (childNode.Name.Equals("field")) {
					Field currentField = IntializeField(childNode);
					result.Fields.Add(currentField);
				}
			}

			return result;
		}

		protected override Form InitializeForm (string formName)
		{
			var root = CurrentXmlForm.DocumentElement;
			var targetCollection = root.SelectSingleNode(String.Format("form[@name='{0}']",formName));
	
			if (targetCollection == null) {
				return null;
			}
	
			return InitializeForm(targetCollection);
		}

		public FormDataModelProvider ()
		{
		}
		
		public override void SaveForm (Form form)
		{
			var root = CurrentXmlForm.DocumentElement;
			var targetCollection = root.SelectSingleNode (String.Format ("form[@name='{0}']", form.Name));
	
			if (targetCollection == null) {
				targetCollection = CurrentXmlForm.CreateNode (XmlNodeType.Element, "form", null);
				
				var idAttr = CurrentXmlForm.CreateAttribute (idAttributeName);
				idAttr.Value = form.Id;
				
				var nameAttr = CurrentXmlForm.CreateAttribute (nameAttributeName);
				nameAttr.Value = form.Name;
				
				targetCollection.Attributes.Append (idAttr);
				targetCollection.Attributes.Append (nameAttr);
				
				PopulateXmlNodeFromForm (targetCollection, form);
				CurrentXmlForm.AppendChild (targetCollection);
			}
			else
			{
				targetCollection.RemoveAll ();
			
				var idAttr = CurrentXmlForm.CreateAttribute (idAttributeName);
				idAttr.Value = form.Id;
				
				var nameAttr = CurrentXmlForm.CreateAttribute (nameAttributeName);
				nameAttr.Value = form.Name;
				
				targetCollection.Attributes.Append (idAttr);
				targetCollection.Attributes.Append (nameAttr);

				PopulateXmlNodeFromForm(targetCollection, form);
			}
			
			CurrentXmlForm.Save(XmlFileName);
		}
		
		private void PopulateXmlNodeFromForm(XmlNode targetNode, Form form)
		{
			foreach (Field field in form.Fields) {
				XmlNode newField = CurrentXmlForm.CreateNode(XmlNodeType.Element, "field", null);
				newField.InnerText = field.Caption;
				
				var idAttr = CurrentXmlForm.CreateAttribute(idAttributeName);
				idAttr.Value = field.Id;
				
				var nameAttr = CurrentXmlForm.CreateAttribute(nameAttributeName);
				nameAttr.Value = field.Name;
				
				var isRequiredAttr = CurrentXmlForm.CreateAttribute(isRequiredAttributeName);
				isRequiredAttr.Value =  field.IsRequired?"true": "false";
				
				var fieldLengthAttr = CurrentXmlForm.CreateAttribute(fieldLengthAttributeName);
				fieldLengthAttr.Value = field.FieldLength.ToString();
				
				var dataTypeAttr = CurrentXmlForm.CreateAttribute(dataTypeAttributeName);
				dataTypeAttr.Value = field.DataType.LookupCode;
				
				newField.Attributes.Append(idAttr);
				newField.Attributes.Append(nameAttr);
				newField.Attributes.Append(isRequiredAttr);
				newField.Attributes.Append(fieldLengthAttr);
				newField.Attributes.Append(dataTypeAttr);
				
				targetNode.AppendChild(newField);
			}
		}
	}
}

