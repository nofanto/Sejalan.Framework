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

using NUnit.Framework;
using System;
using System.Linq;
using System.Configuration;
using Sejalan.Framework.Utility;
using Sejalan.Framework.Provider;
namespace Sejalan.Framework.Data.DbLinq.Sqlite.Test
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
		public void CanGetConnectionStringConfiguration ()
		{
			bool isPassedTest = false;
			foreach (ConnectionStringSettings item in ConfigurationManager.ConnectionStrings) {
				if (item.Name == "Test") {
					isPassedTest = true;
				}
			}
			Assert.AreEqual(true, isPassedTest);
		}
		
		[Test()]
		public void CanGetDataContextProviderConfiguration()
		{
			
			Assert.AreEqual(ProviderFactory.GetInstance<DataContextFactory>(ProviderRepositoryFactory.Instance.Provider).GetDefaultProvider<DataContextProviderBase>().Name, "TestDataContext");
			Assert.AreEqual(ProviderFactory.GetInstance<DataContextFactory>(ProviderRepositoryFactory.Instance.Provider).GetProviders<DataContextProviderBase>()["TestDataContext"].Name, "TestDataContext");
			Assert.AreEqual(ProviderFactory.GetInstance<DataContextFactory>(ProviderRepositoryFactory.Instance.Provider).GetDefaultProvider<DataContextProviderBase>().DatabaseId, "Sqlite");
			Assert.AreEqual(ProviderFactory.GetInstance<DataContextFactory>(ProviderRepositoryFactory.Instance.Provider).GetDefaultProvider<DataContextProviderBase>().ConnectionString, ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
			
		}
		
		
		[Test()]
		public void CanPerformCRUD()
		{
			TestValueObject testTO = null;
			
			using(TestDataContext dataContext = ProviderFactory.GetInstance<DataContextFactory>(ProviderRepositoryFactory.Instance.Provider).GetDefaultProvider<DataContextProviderBase>().DataContext as TestDataContext)
			{
				//test create
				testTO = new TestValueObject();
				testTO.Name = "ASP.NET";
				dataContext.Tests.InsertOnSubmit(testTO);
				dataContext.SubmitChanges();
				Assert.AreNotEqual(testTO.ID, 0);	
			}
			
			//test read
			testTO = GetTest().Where(p => p.Name == "ASP.NET").SingleOrDefault();
			Assert.AreEqual(testTO.Name,"ASP.NET");
			
			//test dynamic type get
			DynamicFieldGetterSetter testValueObjectDynamic = typeof(TestValueObject).GetDynamicFieldGetterSetter();
			Assert.AreEqual(testValueObjectDynamic.GetColumnValue("Name",testTO).ToString(),"ASP.NET");
			
			using(TestDataContext dataContext = ProviderFactory.GetInstance<DataContextFactory>(ProviderRepositoryFactory.Instance.Provider).GetDefaultProvider<DataContextProviderBase>().DataContext as TestDataContext)
			{	
				//test update
				testTO.Name = "C#";
				dataContext.Tests.Attach(testTO);
				dataContext.SubmitChanges();
				
				testTO = dataContext.Tests.Where(p => p.ID == testTO.ID).SingleOrDefault();
				Assert.AreEqual(testTO.Name,"C#");
				
				//test dynamic type set
				testValueObjectDynamic.SetColumnValue("Name",testTO,"ASP.NET");
				dataContext.SubmitChanges();
				
				testTO = dataContext.Tests.Where(p => p.ID == testTO.ID).SingleOrDefault();
				Assert.AreEqual(testTO.Name,"ASP.NET");
				
				//test delete
				dataContext.Tests.DeleteOnSubmit(testTO);
				dataContext.SubmitChanges();
				
				testTO = dataContext.Tests.Where(p => p.Name == "ASP.NET").SingleOrDefault();
				Assert.AreEqual(testTO, null);
			}
		
		}
		
		private IQueryable<TestValueObject> GetTest()
		{
			IQueryable<TestValueObject> queryAbleExpression;
			using(TestDataContext dataContext = ProviderFactory.GetInstance<DataContextFactory>(ProviderRepositoryFactory.Instance.Provider).GetDefaultProvider<DataContextProviderBase>().DataContext as TestDataContext)
			{
				queryAbleExpression = dataContext.Tests;
			}
			
			return queryAbleExpression;
		}
		
		[TestFixtureTearDown()]
		public void TearDown()
		{
			using(TestDataContext dataContext = ProviderFactory.GetInstance<DataContextFactory>(ProviderRepositoryFactory.Instance.Provider).GetDefaultProvider<DataContextProviderBase>().DataContext as TestDataContext)
			{
				dataContext.ExecuteCommand("DELETE FROM Tests");
			}
		}
	}
}


