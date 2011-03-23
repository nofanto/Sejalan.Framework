// 
//  ProviderBase.cs
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
    /// Base class for framework's provider.
    /// </summary>
    public class ProviderBase
    {
        // Fields
        private string _Description;
        private bool _Initialized;
        private string _name;
        private NameValueCollection _Parameters;

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public virtual NameValueCollection Parameters
        {
            get { return _Parameters; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderBase"/> class.
        /// </summary>
        protected ProviderBase()
        {
        }

        /// <summary>
        /// Initializes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="config">The config.</param>
        public virtual void Initialize(string name, NameValueCollection config)
        {
            lock (this)
            {
                if (this._Initialized)
                {
                    throw new InvalidOperationException("ProviderAlreadyInitialized");
                }
                this._Initialized = true;
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name.Length == 0)
            {
                throw new ArgumentException("ConfigProviderNameNullEmpty");
            }
            this._name = name;
            if (config != null)
            {
                this._Description = config["description"];
                config.Remove("description");
            }

            this._Parameters = config;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public virtual string Description
        {
            get
            {
                if (!string.IsNullOrEmpty(this._Description))
                {
                    return this._Description;
                }
                return this.Name;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public virtual string Name
        {
            get
            {
                return this._name;
            }
        }

    }
}

