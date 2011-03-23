// 
//  LookupItemCollection.cs
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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace Sejalan.Framework.LookupDataModel
{
   public class LookupItemCollection : KeyedCollection<string,LookupItem>
   {
		private string _CollectionName = string.Empty;
		
		public String CollectionName {
			get{
				return _CollectionName;}
		}
		public LookupItemCollection(string collectionName) :base()
		{
			_CollectionName= collectionName;
		}
        #region Methods

        public LookupItem GetItemByLookupID(string id)
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentException("id is null or empty.", "id");
            foreach (LookupItem item in this.Items)
            {
                if (string.Compare(item.LookupID, id) == 0)
                    return item;
            }

            return null;
        }

        public Dictionary<string, string> GetLookupList
        {
            get
            {
                Dictionary<string, string> result;

                result = new Dictionary<string, string>();
                foreach (LookupItem lookupValue in this.Items)
                    result.Add(lookupValue.LookupCode, lookupValue.LookupDesc);

                return result;
            }
        }

        
        protected override string GetKeyForItem(LookupItem item)
        {
            return item.LookupCode;
        }
        
        #endregion
		
		        
    }
}

