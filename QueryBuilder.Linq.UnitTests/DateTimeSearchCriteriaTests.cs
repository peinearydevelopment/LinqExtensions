namespace QueryBuilder.Linq.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using QueryBuilder.Contracts;

    [TestClass]
	public class DateTimeSearchCriteriaTests
	{
		private readonly List<TestClass> _testClassEnumeration = new List<TestClass>
		{
			new TestClass
			{
				TestDateTime = DateTime.MinValue
			},
			new TestClass
			{
				TestDateTime = DateTime.MaxValue
			}
		};

		[TestMethod]
		public void DateTimeBeforeTest()
		{
			var searchCriteria = new TestClassSearchCriteria
			{
				TestDateTimeFilter = new DateTimeSearchCriteria { SearchType = DateTimeSearchType.Before, Value = DateTime.MaxValue }
			};

			var searchResults = _testClassEnumeration.Search(searchCriteria);
			Assert.AreEqual(1, searchResults.Count());
			Assert.AreEqual(DateTime.MinValue, searchResults.First().TestDateTime);
		}

		[TestMethod]
		public void DateTimeAfterTest()
		{
			var searchCriteria = new TestClassSearchCriteria
			{
				TestDateTimeFilter = new DateTimeSearchCriteria { SearchType = DateTimeSearchType.After, Value = DateTime.MinValue }
			};

			var searchResults = _testClassEnumeration.Search(searchCriteria);
			Assert.AreEqual(1, searchResults.Count());
			Assert.AreEqual(DateTime.MaxValue, searchResults.First().TestDateTime);
		}

		[TestMethod]
		public void DateTimeBeforeOrEqualsTest()
		{
			var searchCriteria = new TestClassSearchCriteria
			{
				TestDateTimeFilter = new DateTimeSearchCriteria { SearchType = DateTimeSearchType.BeforeOrEquals, Value = DateTime.MaxValue }
			};

			var searchResults = _testClassEnumeration.Search(searchCriteria);
			Assert.AreEqual(2, searchResults.Count());
			Assert.IsTrue(searchResults.Any(result => result.TestDateTime == DateTime.MinValue));
			Assert.IsTrue(searchResults.Any(result => result.TestDateTime == DateTime.MaxValue));
		}

		[TestMethod]
		public void DateTimeAfterOrEqualsTest()
		{
			var searchCriteria = new TestClassSearchCriteria
			{
				TestDateTimeFilter = new DateTimeSearchCriteria { SearchType = DateTimeSearchType.AfterOrEquals, Value = DateTime.MinValue }
			};

			var searchResults = _testClassEnumeration.Search(searchCriteria);
			Assert.AreEqual(2, searchResults.Count());
			Assert.IsTrue(searchResults.Any(result => result.TestDateTime == DateTime.MinValue));
			Assert.IsTrue(searchResults.Any(result => result.TestDateTime == DateTime.MaxValue));
		}

		[TestMethod]
		public void DateTimeEqualsTest()
		{
			var searchCriteria = new TestClassSearchCriteria
			{
				TestDateTimeFilter = new DateTimeSearchCriteria { SearchType = DateTimeSearchType.Equals, Value = DateTime.MinValue }
			};

			var searchResults = _testClassEnumeration.Search(searchCriteria);
			Assert.AreEqual(1, searchResults.Count());
			Assert.AreEqual(DateTime.MinValue, searchResults.First().TestDateTime);
		}

		[TestMethod]
		public void DateTimeNoneTest()
		{
			var searchCriteria = new TestClassSearchCriteria
			{
				TestDateTimeFilter = new DateTimeSearchCriteria { SearchType = DateTimeSearchType.None, Value = DateTime.MinValue }
			};

			var searchResults = _testClassEnumeration.Search(searchCriteria);
			Assert.AreEqual(2, searchResults.Count());
			Assert.IsTrue(searchResults.Any(result => result.TestDateTime == DateTime.MinValue));
			Assert.IsTrue(searchResults.Any(result => result.TestDateTime == DateTime.MaxValue));
		}

		[TestMethod]
		public void DateTimeSearchTypeDoesNotExist()
		{
			var searchCriteria = new TestClassSearchCriteria
			{
				TestDateTimeFilter = new DateTimeSearchCriteria { SearchType = (DateTimeSearchType)613 }
			};

			try
			{
				_testClassEnumeration.Search(searchCriteria);
			}
			catch (ArgumentOutOfRangeException ex)
			{
				Assert.AreEqual("Specified argument was out of the range of valid values.\r\nParameter name: DateTimeSearchType with value 613 hasn't been implemented.", ex.Message);
			}
		}
	}
}
