// 
//  DataContextProviderBase.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sejalan.Framework.Provider;
using System.Configuration;
using DbLinq.Data.Linq;

namespace Sejalan.Framework.Data.DbLinq
{
    /// <summary>
    /// Base class for all DataContextProvider
    /// </summary>
    public abstract class DataContextProviderBase :ProviderBase
    {
        public string ConnectionString { get{return ConfigurationManager.ConnectionStrings[  this.Parameters["connectionStringName"]].ConnectionString;}}
        public string SchemaName {get{return this.Parameters["schemaName"];}}
        public string DatabaseId { get { return this.Parameters["databaseId"]; } }

		protected DataContextProviderBase()
		{
		}
		
        /// <summary>
        /// Gets the <see cref="DataContext"/>.
        /// </summary>
        /// <value>The data context.</value>
        public DataContext DataContext
        {
            get
            {
                return GetDataContext(this.ConnectionString);
            }
        }

        /// <summary>
        /// When overriden in derived class, gets the <see cref="DataContext"/>, using supplied connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        protected abstract DataContext GetDataContext(string connectionString);
       
    }
}
