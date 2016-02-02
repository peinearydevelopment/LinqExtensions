namespace QueryBuilder.EntityFramework.UnitTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using QueryBuilder.Contracts;
    using QueryBuilder.EntityFramework;

    [TestClass]
    public class EfIntegerSearchCriteriaTests
    {
        private const int TestValue = 2;

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
        public async Task IntegerSearchCriteriaNoneShouldReturnAllObjectsInDatabase()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestIntegerProperty = new IntegerSearchCriteria
                    {
                        SearchType = IntegerSearchType.None,
                        Value = TestValue
                    }
                };

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Length, results.Length);
            }
        }

        [TestMethod]
        public async Task IntegerSearchCriteriaLessThanShouldReturnObjectsInDatabaseThatAreLessThan()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestIntegerProperty = new IntegerSearchCriteria
                    {
                        SearchType = IntegerSearchType.LessThan,
                        Value = TestValue
                    }
                };

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestIntegerProperty != null && testobject.TestIntegerProperty < TestValue), results.Length);
            }
        }

        [TestMethod]
        public async Task IntegerSearchCriteriaLessThanOrEqualsShouldReturnObjectsInDatabaseThatAreLessThanOrEquals()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestIntegerProperty = new IntegerSearchCriteria
                    {
                        SearchType = IntegerSearchType.LessThanOrEquals,
                        Value = TestValue
                    }
                };

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestIntegerProperty != null && testobject.TestIntegerProperty <= TestValue), results.Length);
            }
        }

        [TestMethod]
        public async Task IntegerSearchCriteriaEqualsShouldReturnObjectsInDatabaseThatAreEqual()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestIntegerProperty = new IntegerSearchCriteria
                    {
                        SearchType = IntegerSearchType.Equals,
                        Value = TestValue
                    }
                };

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestIntegerProperty != null && testobject.TestIntegerProperty == TestValue), results.Length);
            }
        }

        [TestMethod]
        public async Task IntegerSearchCriteriaGreaterThanShouldReturnObjectsInDatabaseThatAreGreaterThan()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestIntegerProperty = new IntegerSearchCriteria
                    {
                        SearchType = IntegerSearchType.GreaterThan,
                        Value = TestValue
                    }
                };

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestIntegerProperty != null && testobject.TestIntegerProperty > TestValue), results.Length);
            }
        }

        [TestMethod]
        public async Task IntegerSearchCriteriaGreaterThanOrEqualsShouldReturnObjectsInDatabaseThatAreGreaterThanOrEqual()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestIntegerProperty = new IntegerSearchCriteria
                    {
                        SearchType = IntegerSearchType.GreaterThanOrEquals,
                        Value = TestValue
                    }
                };

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestIntegerProperty != null && testobject.TestIntegerProperty >= TestValue), results.Length);
            }
        }

        [TestMethod]
        public async Task IntegerSearchCriteriaDoesNotEqualShouldReturnObjectsInDatabaseThatDoNotEqual()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestIntegerProperty = new IntegerSearchCriteria
                    {
                        SearchType = IntegerSearchType.DoesNotEqual,
                        Value = TestValue
                    }
                };

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestIntegerProperty != null && testobject.TestIntegerProperty != TestValue), results.Length);
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
