namespace QueryBuilder.EntityFramework.UnitTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using QueryBuilder.Contracts;
    using QueryBuilder.EntityFramework;

    [TestClass]
    public class EfStringSearchCriteriaTests
    {
        //TODO: research below approach to get more information about the fluent interface
        //ObjectContext objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
        //var ba = objectContext.CreateObjectSet<TestObject>().ToTraceString();
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
        public async Task StringSearchCriteriaNoneShouldReturnAllObjectsInDatabase()
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

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Length, results.Length);
            }
        }

        [TestMethod]
        public async Task StringSearchCriteriaStartsWithShouldReturnObjectsInDatabaseThatStartWithA()
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

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestStringProperty != null && testobject.TestStringProperty.StartsWith("a", StringComparison.OrdinalIgnoreCase)), results.Length);
            }
        }

        [TestMethod]
        public async Task StringSearchCriteriaEndsWithShouldReturnObjectsInDatabaseThatEndWithA()
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

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestStringProperty != null && testobject.TestStringProperty.EndsWith("a", StringComparison.OrdinalIgnoreCase)), results.Length);
            }
        }

        [TestMethod]
        public async Task StringSearchCriteriaContainsShouldReturnObjectsInDatabaseThatContainA()
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

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestStringProperty != null && testobject.TestStringProperty.Contains("a")), results.Length);
            }
        }

        [TestMethod]
        public async Task StringSearchCriteriaEqualsShouldReturnObjectsInDatabaseThatEqualA()
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

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestStringProperty != null && testobject.TestStringProperty.Equals("a", StringComparison.OrdinalIgnoreCase)), results.Length);
            }
        }

        [TestMethod]
        public async Task StringSearchCriteriaDoesNotEqualShouldReturnObjectsInDatabaseThatDoesNotEqualA()
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

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestStringProperty != null && !testobject.TestStringProperty.Equals("a", StringComparison.OrdinalIgnoreCase)), results.Length);
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
