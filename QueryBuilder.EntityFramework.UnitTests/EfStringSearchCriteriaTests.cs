namespace QueryBuilder.EntityFramework.UnitTests
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using QueryBuilder.Contracts;

    [TestClass]
    public class EfStringSearchCriteriaTests
    {
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            using (var dbContext = new TestDbContext())
            {
                dbContext.TestObjects.RemoveRange(dbContext.TestObjects.ToArray());
                TestData.TestObjects = dbContext.TestObjects.AddRange(TestData.TestObjects).ToArray();
                dbContext.SaveChanges();
            }
        }

        [TestMethod]
        public void EmptySearchCriteriaObjectShouldReturnAllObjectsInDatabase()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria();
                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Length, result.Results.Count);
            }
        }

        [TestMethod]
        public void StringSearchCriteriaNoneShouldReturnAllObjectsInDatabase()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestStringProperty = new StringSearchCriteria
                    {
                        SearchType = StringSearchType.None,
                        Value = "aa"
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Length, result.Results.Count);
            }
        }

        [TestMethod]
        public void StringSearchCriteriaStartsWithShouldReturnObjectsInDatabaseThatStartWithA()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestStringProperty = new StringSearchCriteria
                    {
                        SearchType = StringSearchType.StartsWith,
                        Value = "a"
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestStringProperty != null && testobject.TestStringProperty.StartsWith("a", StringComparison.OrdinalIgnoreCase)), result.Results.Count);
            }
        }

        [TestMethod]
        public void StringSearchCriteriaEndsWithShouldReturnObjectsInDatabaseThatEndWithA()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestStringProperty = new StringSearchCriteria
                    {
                        SearchType = StringSearchType.EndsWith,
                        Value = "a"
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestStringProperty != null && testobject.TestStringProperty.EndsWith("a", StringComparison.OrdinalIgnoreCase)), result.Results.Count);
            }
        }

        [TestMethod]
        public void StringSearchCriteriaContainsShouldReturnObjectsInDatabaseThatContainA()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestStringProperty = new StringSearchCriteria
                    {
                        SearchType = StringSearchType.Contains,
                        Value = "a"
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestStringProperty != null && testobject.TestStringProperty.Contains("a")), result.Results.Count);
            }
        }

        [TestMethod]
        public void StringSearchCriteriaEqualsShouldReturnObjectsInDatabaseThatEqualA()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestStringProperty = new StringSearchCriteria
                    {
                        SearchType = StringSearchType.Equals,
                        Value = "a"
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestStringProperty != null && testobject.TestStringProperty.Equals("a", StringComparison.OrdinalIgnoreCase)), result.Results.Count);
            }
        }

        [TestMethod]
        public void StringSearchCriteriaDoesNotEqualShouldReturnObjectsInDatabaseThatDoesNotEqualA()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestStringProperty = new StringSearchCriteria
                    {
                        SearchType = StringSearchType.DoesNotEqual,
                        Value = "a"
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestStringProperty != null && !testobject.TestStringProperty.Equals("a", StringComparison.OrdinalIgnoreCase)), result.Results.Count);
            }
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            using (var dbContext = new TestDbContext())
            {
                dbContext.TestObjects.RemoveRange(dbContext.TestObjects.ToArray());
                dbContext.SaveChanges();
            }
        }
    }
}
