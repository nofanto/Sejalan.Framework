// 
//  PreferenceItemCollection.cs
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

namespace Sejalan.Framework.Preferences
{
	public class PreferenceItemCollection: KeyedCollection<string,PreferenceItem>
	{
		private string _CollectionName = string.Empty;

		public String CollectionName {
			get {
				return _CollectionName;
			}
		}
		
		
		protected override void InsertItem (int index, PreferenceItem item)
		{
			item.CollectionName = _CollectionName;
			base.InsertItem (index, item);
		}
		
		
		public PreferenceItemCollection (string collectionName) :base()
		{
			_CollectionName = collectionName;
		}

		#region Methods

		public PreferenceItem GetItemByLookupID (string id)
		{
			if (String.IsNullOrEmpty (id))
				throw new ArgumentException ("id is null or empty.", "id");
			foreach (PreferenceItem item in this.Items) {
				if (string.Compare (item.ID, id) == 0)
					return item;
			}

			return null;
		}

		protected override string GetKeyForItem (PreferenceItem item)
		{
			return item.Name;
		}

		#endregion
		
	}
}

