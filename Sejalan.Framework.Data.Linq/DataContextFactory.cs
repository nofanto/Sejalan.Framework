// 
//  DataContextFactory.cs
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
using System.Configuration;
using Sejalan.Framework.Provider;

namespace Sejalan.Framework.Data.Linq
{
    /// <summary>
    /// Factory for <see cref="DataContextProvider"/>
    /// </summary>
    public class DataContextFactory: ProviderFactory
    {
		
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContextFactory"/> class.
        /// </summary>
        public DataContextFactory()
            : base()
        {


        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        protected override string Key
        {
            get { return "sejalan.framework/dataContext"; }
        }

    }
}
