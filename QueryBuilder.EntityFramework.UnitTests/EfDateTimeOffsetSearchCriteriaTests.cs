namespace QueryBuilder.EntityFramework.UnitTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Framework.Searching.EntityFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using QueryBuilder.Contracts;

    [TestClass]
    public class EfDateTimeOffsetSearchCriteriaTests
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
        public async Task EmptySearchCriteriaObjectShouldReturnAllObjectsInDatabase()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria();
                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Length, results.Length);
            }
        }

        [TestMethod]
        public async Task DateTimeOffsetSearchCriteriaNoneShouldReturnAllObjectsInDatabase()
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

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Length, results.Length);
            }
        }

        [TestMethod]
        public async Task DateTimeOffsetSearchCriteriaBeforeShouldReturnObjectsInDatabaseThatAreBefore()
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

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeOffsetProperty != null && testobject.TestDateTimeOffsetProperty < TestData.DateTimeOffsetNow), results.Length);
            }
        }

        [TestMethod]
        public async Task DateTimeOffsetSearchCriteriaBeforeOrEqualsShouldReturnObjectsInDatabaseThatAreBeforeOrEquals()
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

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeOffsetProperty != null && testobject.TestDateTimeOffsetProperty <= TestData.DateTimeOffsetNow), results.Length);
            }
        }

        [TestMethod]
        public async Task DateTimeOffsetSearchCriteriaEqualsShouldReturnObjectsInDatabaseThatAreEqual()
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

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeOffsetProperty != null && testobject.TestDateTimeOffsetProperty == TestData.DateTimeOffsetNow), results.Length);
            }
        }

        [TestMethod]
        public async Task DateTimeOffsetSearchCriteriaAfterShouldReturnObjectsInDatabaseThatAreAfter()
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

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeOffsetProperty != null && testobject.TestDateTimeOffsetProperty > TestData.DateTimeOffsetNow), results.Length);
            }
        }

        [TestMethod]
        public async Task DateTimeOffsetSearchCriteriaAfterOrEqualsShouldReturnObjectsInDatabaseThatAreAfterOrEquals()
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

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeOffsetProperty != null && testobject.TestDateTimeOffsetProperty >= TestData.DateTimeOffsetNow), results.Length);
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
