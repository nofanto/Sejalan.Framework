// 
//  ProviderSettings.cs
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
using System.Collections.Specialized;
namespace Sejalan.Framework.Provider
{
	 /// <summary>
    /// Class for handling framework's provider configuration entry.
    /// </summary>
    public class ProviderSettings 
    {
        // Fields
        private NameValueCollection _PropertyNameCollection = new NameValueCollection();

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderSettings"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        public ProviderSettings(string name, string type)
        {
            this._PropertyNameCollection.Add("name", name);
            this._PropertyNameCollection.Add("type", type);
        }


        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return (string) this._PropertyNameCollection["name"];
            }
            set
            {
                this._PropertyNameCollection["name"] = value;
            }
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public NameValueCollection Parameters
        {
            get
            {
               return this._PropertyNameCollection;
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get
            {
                return this._PropertyNameCollection["type"];
            }
            set
            {
               this._PropertyNameCollection["type"] = value;
            }
        }

    }
}

