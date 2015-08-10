using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder;

namespace UnitTests
{
	[TestClass]
	public class UnitTest1
	{
		private List<TestClass> TestClassEnumeration = new List<TestClass>
		{
			new TestClass
			{
				TestDateTime = DateTime.MinValue,
				TestString = "AString"
			},
			new TestClass
			{
				TestDateTime = DateTime.MaxValue,
				TestString = "ZString"
			}
		};

		[TestMethod]
		public void TestMethod1()
		{
			var searchCriteria = new TestClassSearchCriteria
			{
				TestStringFilter = new StringSearchCriteria { SearchType = StringSearchType.StartsWith, Value = "A" }//,
				//TestDateTimeFilter = new DateTimeSearchCriteria { SearchType = DateTimeSearchType.Before, Value = DateTime.MaxValue }
			};

			var searchResults = TestClassEnumeration.Search(searchCriteria);
			Assert.AreEqual(1, searchResults.Count());
		}

		[TestMethod]
		public void TestMethod2()
		{
			var searchCriteria = new TestClassSearchCriteria
			{
				//TestStringFilter = new StringSearchCriteria { SearchType = StringSearchType.StartsWith, Value = "A" },
				TestDateTimeFilter = new DateTimeSearchCriteria { SearchType = DateTimeSearchType.Before, Value = DateTime.MaxValue }
			};

			var searchResults = TestClassEnumeration.Search(searchCriteria);
			Assert.AreEqual(1, searchResults.Count());
		}
	}
}
