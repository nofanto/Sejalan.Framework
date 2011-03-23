// 
//  Field.cs
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
using System.ComponentModel;
using Sejalan.Framework.LookupDataModel;
namespace Sejalan.Framework.FormDataModel
{
	public class Field
	{
		public Field ()
		{
		}
		
		private string _Id;
        [Browsable(false)]
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _Name;
        [ReadOnly(true)]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

		private string _Caption;
		[Browsable(true)]
		public string Caption
		{
			get { return _Caption; }
			set { _Caption = value; }
		}

        private bool _IsRequired;
        [Browsable(true)]
        public bool IsRequired
        {
            get { return _IsRequired; }
            set { _IsRequired = value; }
        }

        private int _FieldLength;
        [ReadOnly(true)]
        public int FieldLength
        {
            get { return _FieldLength; }
            set { _FieldLength = value; }
        }

        private LookupItem _DataType;
        [ReadOnly(true)]
        public LookupItem DataType { get { return _DataType; } set { _DataType = value; } }

	}
	
}

