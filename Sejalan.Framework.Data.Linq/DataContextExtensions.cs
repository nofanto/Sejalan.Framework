// 
//  DataContextExtensions.cs
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
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;

namespace Sejalan.Framework.Data.Linq
{
    public static class Extensions
    {
        public static void BulkInsert<TEntity>(this Table<TEntity> table, IEnumerable<TEntity> entities) where TEntity : class
        {
            SqlBulkCopy bulk = new SqlBulkCopy(table.Context.Connection.ConnectionString);
            var entityType = entities.GetType().GetInterface("IEnumerable`1").GetGenericArguments()[0];  
            String _TableName = (entityType.GetCustomAttributes(typeof(TableAttribute), false) as TableAttribute[])[0].Name; 
            ValueObjectDataReader<TEntity> reader = new ValueObjectDataReader<TEntity>(entities);
            for (int i = 0; i < reader.FieldCount; i++)
            {
                String columnName = reader.GetName(i);
                bulk.ColumnMappings.Add(columnName, columnName);
            }
            bulk.DestinationTableName = _TableName;
            bulk.WriteToServer(reader as IDataReader);
        }
    }
}
