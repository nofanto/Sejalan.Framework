// 
//  ProviderConfigurationSectionHandler.cs
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
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;

namespace Sejalan.Framework.Provider.AppConfig
{
	  /// <summary>
    /// Class for handle the framework's provider configuration.
    /// </summary>
    public class ProviderConfigurationSectionHandler : IConfigurationSectionHandler
    {
        private Dictionary<string,ProviderSettings> _Providers;
        private string _DefaultProviderName;

        private NameValueCollection _Parameters = new NameValueCollection();

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public virtual NameValueCollection Parameters
        {
            get { return _Parameters; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderConfigurationSectionHandler"/> class.
        /// </summary>
        public ProviderConfigurationSectionHandler()
        {
            _Providers = new Dictionary<string,ProviderSettings>();
        }

        #region IConfigurationSectionHandler Members

        /// <summary>
        /// Creates a configuration section handler.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext">Configuration context object.</param>
        /// <param name="section"></param>
        /// <returns>The created section handler object.</returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            if (section == null) throw new ArgumentNullException("section");

            XmlAttributeCollection attributeCollection = section.Attributes;

            // Read child nodes
            foreach (XmlNode child in section.ChildNodes)
            {
                if (child.Name == "providers")
                    GetProvider(child);
            }

            // Get the default dataaccess
            _DefaultProviderName = attributeCollection["defaultProvider"].Value;

            foreach (XmlAttribute item in attributeCollection)
            {
                _Parameters.Add(item.Name, item.Value);
            }

            return this;

        }

        #endregion

        /// <summary>
        /// Gets the name of the default provider.
        /// </summary>
        /// <value>The name of the default provider.</value>
        public String DefaultProviderName
        {
            get
            { return _DefaultProviderName; }
        }

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <value>The providers.</value>
        public Dictionary<string,ProviderSettings> Providers
        {
            get
            {
                return  _Providers;
            }
        }

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <param name="providers">The providers.</param>
        protected virtual void GetProvider(XmlNode providers)
        {
            foreach (XmlNode provider in providers.ChildNodes)
            {
                switch (provider.Name)
                {
                    case "add":
                        AddElementToCollection(provider.Attributes["name"].Value, provider.Attributes);
                        break;

                    case "remove":
                        _Providers.Remove(provider.Attributes["name"].Value);
                        break;

                    case "clear":
                        _Providers.Clear();
                        break;
                }
            }

        }

        /// <summary>
        /// Adds the element to collection.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="attributes">The attributes.</param>
        protected virtual void AddElementToCollection(string name, XmlAttributeCollection attributes)
        {
            ProviderSettings newProviderSettings = new ProviderSettings(name, attributes["type"].Value);
            foreach (XmlAttribute attribute in attributes)
            {

                if (attribute.Name != "name" && attribute.Name != "type")
                    newProviderSettings.Parameters.Add(attribute.Name, attribute.Value);

            }

			_Providers.Add(newProviderSettings.Name, newProviderSettings);
           
        }
    }

}

