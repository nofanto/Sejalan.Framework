// 
//  ProviderDataContextProvider.cs
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
using Sejalan.Framework.Data.DbLinq;
using Mono.Data.Sqlite;
using DbLinq.Data.Linq;

namespace Sejalan.Framework.Provider.Data.DbLinq.Sqlite
{
	public class ProviderDataContextProvider : DataContextProviderBase
	{
		#region implemented abstract members of Sejalan.Framework.Data.DbLinq.DataContextProviderBase
		protected override DataContext GetDataContext (string connectionString)
		{
			return new ProviderDataContext(new SqliteConnection(connectionString));
		}
		
		#endregion
		public ProviderDataContextProvider ()
		{
		}
	}
}

