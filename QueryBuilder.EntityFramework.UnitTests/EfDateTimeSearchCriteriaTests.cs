namespace QueryBuilder.EntityFramework.UnitTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Framework.Searching.EntityFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using QueryBuilder.Contracts;

    [TestClass]
    public class EfDateTimeSearchCriteriaTests
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
        public async Task DateTimeSearchCriteriaNoneShouldReturnAllObjectsInDatabase()
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

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Length, results.Length);
            }
        }

        [TestMethod]
        public async Task DateTimeSearchCriteriaBeforeShouldReturnObjectsInDatabaseThatAreBefore()
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

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeProperty != null && testobject.TestDateTimeProperty < TestData.DateTimeNow), results.Length);
            }
        }

        [TestMethod]
        public async Task DateTimeSearchCriteriaBeforeOrEqualsShouldReturnObjectsInDatabaseThatAreBeforeOrEquals()
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

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeProperty != null && testobject.TestDateTimeProperty <= TestData.DateTimeNow), results.Length);
            }
        }

        [TestMethod]
        public async Task DateTimeSearchCriteriaEqualsShouldReturnObjectsInDatabaseThatAreEqual()
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

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeProperty != null && testobject.TestDateTimeProperty == TestData.DateTimeNow), results.Length);
            }
        }

        [TestMethod]
        public async Task DateTimeSearchCriteriaAfterShouldReturnObjectsInDatabaseThatAreAfter()
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

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeProperty != null && testobject.TestDateTimeProperty > TestData.DateTimeNow), results.Length);
            }
        }

        [TestMethod]
        public async Task DateTimeSearchCriteriaAfterOrEqualsShouldReturnObjectsInDatabaseThatAreAfterOrEquals()
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

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDateTimeProperty != null && testobject.TestDateTimeProperty >= TestData.DateTimeNow), results.Length);
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
