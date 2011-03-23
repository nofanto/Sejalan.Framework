// 
//  CacheProvider.cs
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
namespace Sejalan.Framework.Cache.Dummy
{
	public class CacheProvider : CacheProviderBase
	{
		private Dictionary<string, object> m_Cache = new Dictionary<string, object>();
		
		#region implemented abstract members of Sejalan.Library.Cache.CacheProviderBase
		public override bool Contains (string key)
		{
			return m_Cache.ContainsKey(String.Format("{0}{1}", this.CacheCategory, key));
		}
		
		
		public override object Read (string key)
		{
			return m_Cache[String.Format("{0}{1}", this.CacheCategory, key)];
		}
		
		
		public override void Update (string key, object value)
		{
			if(m_Cache.ContainsKey(String.Format("{0}{1}", this.CacheCategory, key)))
				m_Cache[String.Format("{0}{1}", this.CacheCategory, key)] = value;
			else
				m_Cache.Add(String.Format("{0}{1}", this.CacheCategory, key),value);
		}
		
		
		public override void Delete (string key)
		{
			m_Cache.Remove(String.Format("{0}{1}", this.CacheCategory, key));
		}
		
		#endregion
		public CacheProvider ()
		{
		}
	}
}

