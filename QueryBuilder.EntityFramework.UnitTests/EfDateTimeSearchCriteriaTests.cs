﻿namespace QueryBuilder.EntityFramework.UnitTests
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Contracts;

    [TestClass]
    public class EfDateTimeSearchCriteriaTests
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
        public void DateTimeSearchCriteriaNoneShouldReturnAllObjectsInDatabase()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestDateTimeProperty = new DateTimeSearchCriteria
                    {
                        SearchType = DateTimeSearchType.None,
                        Value = TestData.DateTimeNow
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Length, result.Results.Count);
            }
        }

        [TestMethod]
        public void DateTimeSearchCriteriaBeforeShouldReturnObjectsInDatabaseThatAreBefore()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestDateTimeProperty = new DateTimeSearchCriteria
                    {
                        SearchType = DateTimeSearchType.Before,
                        Value = TestData.DateTimeNow
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeProperty != null && testobject.TestDateTimeProperty < TestData.DateTimeNow), result.Results.Count);
            }
        }

        [TestMethod]
        public void DateTimeSearchCriteriaBeforeOrEqualsShouldReturnObjectsInDatabaseThatAreBeforeOrEquals()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestDateTimeProperty = new DateTimeSearchCriteria
                    {
                        SearchType = DateTimeSearchType.BeforeOrEquals,
                        Value = TestData.DateTimeNow
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeProperty != null && testobject.TestDateTimeProperty <= TestData.DateTimeNow), result.Results.Count);
            }
        }

        [TestMethod]
        public void DateTimeSearchCriteriaEqualsShouldReturnObjectsInDatabaseThatAreEqual()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestDateTimeProperty = new DateTimeSearchCriteria
                    {
                        SearchType = DateTimeSearchType.Equals,
                        Value = TestData.DateTimeNow
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeProperty != null && testobject.TestDateTimeProperty == TestData.DateTimeNow), result.Results.Count);
            }
        }

        [TestMethod]
        public void DateTimeSearchCriteriaAfterShouldReturnObjectsInDatabaseThatAreAfter()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestDateTimeProperty = new DateTimeSearchCriteria
                    {
                        SearchType = DateTimeSearchType.After,
                        Value = TestData.DateTimeNow
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeProperty != null && testobject.TestDateTimeProperty > TestData.DateTimeNow), result.Results.Count);
            }
        }

        [TestMethod]
        public void DateTimeSearchCriteriaAfterOrEqualsShouldReturnObjectsInDatabaseThatAreAfterOrEquals()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestDateTimeProperty = new DateTimeSearchCriteria
                    {
                        SearchType = DateTimeSearchType.AfterOrEquals,
                        Value = TestData.DateTimeNow
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeProperty != null && testobject.TestDateTimeProperty >= TestData.DateTimeNow), result.Results.Count);
            }
        }
    }
}
