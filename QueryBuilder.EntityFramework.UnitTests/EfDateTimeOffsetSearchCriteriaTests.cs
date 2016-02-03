namespace QueryBuilder.EntityFramework.UnitTests
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Contracts;

    [TestClass]
    public class EfDateTimeOffsetSearchCriteriaTests
    {
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
        public void DateTimeOffsetSearchCriteriaNoneShouldReturnAllObjectsInDatabase()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestDateTimeOffsetProperty = new DateTimeOffsetSearchCriteria
                    {
                        SearchType = DateTimeOffsetSearchType.None,
                        Value = TestData.DateTimeOffsetNow
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Length, result.Results.Count);
            }
        }

        [TestMethod]
        public void DateTimeOffsetSearchCriteriaBeforeShouldReturnObjectsInDatabaseThatAreBefore()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestDateTimeOffsetProperty = new DateTimeOffsetSearchCriteria
                    {
                        SearchType = DateTimeOffsetSearchType.Before,
                        Value = TestData.DateTimeOffsetNow
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeOffsetProperty != null && testobject.TestDateTimeOffsetProperty < TestData.DateTimeOffsetNow), result.Results.Count);
            }
        }

        [TestMethod]
        public void DateTimeOffsetSearchCriteriaBeforeOrEqualsShouldReturnObjectsInDatabaseThatAreBeforeOrEquals()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestDateTimeOffsetProperty = new DateTimeOffsetSearchCriteria
                    {
                        SearchType = DateTimeOffsetSearchType.BeforeOrEquals,
                        Value = TestData.DateTimeOffsetNow
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeOffsetProperty != null && testobject.TestDateTimeOffsetProperty <= TestData.DateTimeOffsetNow), result.Results.Count);
            }
        }

        [TestMethod]
        public void DateTimeOffsetSearchCriteriaEqualsShouldReturnObjectsInDatabaseThatAreEqual()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestDateTimeOffsetProperty = new DateTimeOffsetSearchCriteria
                    {
                        SearchType = DateTimeOffsetSearchType.Equals,
                        Value = TestData.DateTimeOffsetNow
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeOffsetProperty != null && testobject.TestDateTimeOffsetProperty == TestData.DateTimeOffsetNow), result.Results.Count);
            }
        }

        [TestMethod]
        public void DateTimeOffsetSearchCriteriaAfterShouldReturnObjectsInDatabaseThatAreAfter()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestDateTimeOffsetProperty = new DateTimeOffsetSearchCriteria
                    {
                        SearchType = DateTimeOffsetSearchType.After,
                        Value = TestData.DateTimeOffsetNow
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeOffsetProperty != null && testobject.TestDateTimeOffsetProperty > TestData.DateTimeOffsetNow), result.Results.Count);
            }
        }

        [TestMethod]
        public void DateTimeOffsetSearchCriteriaAfterOrEqualsShouldReturnObjectsInDatabaseThatAreAfterOrEquals()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestDateTimeOffsetProperty = new DateTimeOffsetSearchCriteria
                    {
                        SearchType = DateTimeOffsetSearchType.AfterOrEquals,
                        Value = TestData.DateTimeOffsetNow
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeOffsetProperty != null && testobject.TestDateTimeOffsetProperty >= TestData.DateTimeOffsetNow), result.Results.Count);
            }
        }
    }
}
