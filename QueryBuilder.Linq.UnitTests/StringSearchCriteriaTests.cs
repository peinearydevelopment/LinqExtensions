namespace QueryBuilder.Linq.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using QueryBuilder.Contracts;

    [TestClass]
	public class StringSearchCriteriaTests
	{
		private readonly List<TestClass> _testClassEnumeration = new List<TestClass>
		{
			new TestClass
			{
				TestString = "AString"
			},
			new TestClass
			{
				TestString = "BString"
			},
			new TestClass
			{
				TestString = "CString"
			},
			new TestClass
			{
				TestString = "DString"
			},
			new TestClass
			{
				TestString = "EString"
			},
			new TestClass
			{
				TestString = "FString"
			},
			new TestClass
			{
				TestString = "GString"
			},
			new TestClass
			{
				TestString = "HString"
			},
			new TestClass
			{
				TestString = "IString"
			},
			new TestClass
			{
				TestString = "JString"
			},
			new TestClass
			{
				TestString = "KString"
			},
			new TestClass
			{
				TestString = "LString"
			},
			new TestClass
			{
				TestString = "MString"
			},
			new TestClass
			{
				TestString = "NString"
			},
			new TestClass
			{
				TestString = "OString"
			},
			new TestClass
			{
				TestString = "PString"
			},
			new TestClass
			{
				TestString = "QString"
			},
			new TestClass
			{
				TestString = "RString"
			},
			new TestClass
			{
				TestString = "SString"
			},
			new TestClass
			{
				TestString = "TString"
			},
			new TestClass
			{
				TestString = "UString"
			},
			new TestClass
			{
				TestString = "VString"
			},
			new TestClass
			{
				TestString = "WString"
			},
			new TestClass
			{
				TestString = "XString"
			},
			new TestClass
			{
				TestString = "YString"
			},
			new TestClass
			{
				TestString = "ZString"
			}
		};

		[TestMethod]
		public void StringStartsWithTest()
		{
			var searchCriteria = new TestClassSearchCriteria
			{
				TestStringFilter = new StringSearchCriteria { SearchType = StringSearchType.StartsWith, Value = "A" }
			};

			var searchResults = _testClassEnumeration.Search(searchCriteria);
			Assert.AreEqual(1, searchResults.Count());
			Assert.AreEqual("AString", searchResults.First().TestString);
		}

		[TestMethod]
		public void StringEndsWithTest()
		{
			var searchCriteria = new TestClassSearchCriteria
			{
				TestStringFilter = new StringSearchCriteria { SearchType = StringSearchType.EndsWith, Value = "String" }
			};

			var searchResults = _testClassEnumeration.Search(searchCriteria);
			Assert.AreEqual(26, searchResults.Count());
		}

		[TestMethod]
		public void StringContainsTest()
		{
			var searchCriteria = new TestClassSearchCriteria
			{
				TestStringFilter = new StringSearchCriteria { SearchType = StringSearchType.Contains, Value = "CS" }
			};

			var searchResults = _testClassEnumeration.Search(searchCriteria);
			Assert.AreEqual(1, searchResults.Count());
			Assert.AreEqual("CString", searchResults.First().TestString);
		}

		[TestMethod]
		public void StringDoesNotEqualTest()
		{
			var searchCriteria = new TestClassSearchCriteria
			{
				TestStringFilter = new StringSearchCriteria { SearchType = StringSearchType.DoesNotEqual, Value = "DString" }
			};

			var searchResults = _testClassEnumeration.Search(searchCriteria);
			Assert.AreEqual(25, searchResults.Count());
		}

		[TestMethod]
		public void StringEqualsTest()
		{
			var searchCriteria = new TestClassSearchCriteria
			{
				TestStringFilter = new StringSearchCriteria { SearchType = StringSearchType.Equals, Value = "DString" }
			};

			var searchResults = _testClassEnumeration.Search(searchCriteria);
			Assert.AreEqual(1, searchResults.Count());
			Assert.AreEqual("DString", searchResults.First().TestString);
		}

		[TestMethod]
		public void StringNoneTest()
		{
			var searchCriteria = new TestClassSearchCriteria
			{
				TestStringFilter = new StringSearchCriteria { SearchType = StringSearchType.None }
			};

			var searchResults = _testClassEnumeration.Search(searchCriteria);
			Assert.AreEqual(26, searchResults.Count());
		}

		[TestMethod]
		public void StringSearchTypeDoesNotExist()
		{
			var searchCriteria = new TestClassSearchCriteria
			{
				TestStringFilter = new StringSearchCriteria { SearchType = (StringSearchType) 613 }
			};

			try
			{
				_testClassEnumeration.Search(searchCriteria);
			}
			catch (ArgumentOutOfRangeException ex)
			{
				Assert.AreEqual("Specified argument was out of the range of valid values.\r\nParameter name: StringSearchType with value 613 hasn't been implemented.", ex.Message);
			}
		}
	}
}
