// 
//  LookupItem.cs
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
namespace Sejalan.Framework.LookupDataModel
{
   public class LookupItem
    {

        #region Fields

        private string _LookupID;
        private string _LookupCode;
        private string _LookupDesc;
        private int _ViewOrder;
 
        #endregion

        #region Constructors

        public LookupItem()
	    {
        }

        #endregion

        #region Properties
        
        public string LookupID
        {
            get
            {
                return this._LookupID;
            }
            set
            {
                this._LookupID = value;
            }
        }
        
        public string LookupCode
        {
            get
            {
                return this._LookupCode;
            }
            set
            {
                this._LookupCode = value;
            }
        }
                
        public string LookupDesc
        {
            get
            {
                return this._LookupDesc;
            }
            set
            {
                this._LookupDesc = value;
            }
        }

        public int ViewOrder
        {
            get
            {
                return this._ViewOrder;
            }
            set
            {
                this._ViewOrder = value;
            }
        }
 
		public string CollectionName {
			get;
			set;
		}
        #endregion
        
    }
}

