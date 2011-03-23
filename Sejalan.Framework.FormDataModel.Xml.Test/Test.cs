// 
//  Test.cs
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
using NUnit.Framework;
using Sejalan.Framework.Provider;
using Sejalan.Framework.Provider.AppConfig;
using Sejalan.Framework.Utility;
namespace Sejalan.Framework.FormDataModel.Xml.Test
{
	[TestFixture()]
	public class Test
	{
		[TestFixtureSetUp()]
		public void SetUp()
		{
			ThreadLocalStorageProviderFactory.Current = new ThreadingThreadLocalStorageProvider();
		}
		
		[Test()]
		public void CanGetFormDataModelConfiguration ()
		{
			FormDataModelProvider formDataModelProvider = ProviderFactory.GetInstance<FormDataModelFactory>(ProviderRepositoryFactory.Instance.Provider).GetDefaultProvider<FormDataModelProvider>();
			Assert.AreEqual(formDataModelProvider.Name, "FormDataModelTest");
			Assert.AreEqual(formDataModelProvider.XmlFileName, "FormDataModel.xml");

		}
		
		
		[Test()]
		public void CanReadAddRemoveField ()
		{
			//TODO: implement add remove field test.
			FormDataModelProvider formDataModelProvider = ProviderFactory.GetInstance<FormDataModelFactory> (ProviderRepositoryFactory.Instance.Provider).GetDefaultProvider<FormDataModelProvider> ();
			
			Form currentForm = formDataModelProvider.GetForm ("subripSubtitleForm");
			Assert.AreEqual (currentForm.Id, "1");
			Assert.AreEqual (currentForm.Fields.Count, 3);
			Assert.AreEqual (currentForm.Fields[1].Name, "endTime");
			Assert.AreEqual (currentForm.Fields[0].DataType.LookupDesc, "TimeSpan");
			Assert.AreEqual (currentForm.Fields[0].IsRequired, true);
			
			//add new field
			Field newField = new Field ();

		}
		
		
	}
}

