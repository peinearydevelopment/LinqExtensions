namespace QueryBuilder.EntityFramework.UnitTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Framework.Searching.EntityFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using QueryBuilder.Contracts;

    [TestClass]
    public class EfDecimalSearchCriteriaTests
    {
        private const decimal TestValue = (decimal)2.5;

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
        public async Task DecimalSearchCriteriaNoneShouldReturnAllObjectsInDatabase()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestDecimalProperty = new DecimalSearchCriteria
                    {
                        SearchType = DecimalSearchType.None,
                        Value = TestValue
                    }
                };

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Length, results.Length);
            }
        }

        [TestMethod]
        public async Task DecimalSearchCriteriaLessThanShouldReturnObjectsInDatabaseThatAreLessThan()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestDecimalProperty = new DecimalSearchCriteria
                    {
                        SearchType = DecimalSearchType.LessThan,
                        Value = TestValue
                    }
                };

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDecimalProperty != null && testobject.TestDecimalProperty < TestValue), results.Length);
            }
        }

        [TestMethod]
        public async Task DecimalSearchCriteriaLessThanOrEqualsShouldReturnObjectsInDatabaseThatAreLessThanOrEquals()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestDecimalProperty = new DecimalSearchCriteria
                    {
                        SearchType = DecimalSearchType.LessThanOrEquals,
                        Value = TestValue
                    }
                };

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDecimalProperty != null && testobject.TestDecimalProperty <= TestValue), results.Length);
            }
        }

        [TestMethod]
        public async Task DecimalSearchCriteriaEqualsShouldReturnObjectsInDatabaseThatAreEqual()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestDecimalProperty = new DecimalSearchCriteria
                    {
                        SearchType = DecimalSearchType.Equals,
                        Value = TestValue
                    }
                };

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDecimalProperty != null && testobject.TestDecimalProperty == TestValue), results.Length);
            }
        }

        [TestMethod]
        public async Task DecimalSearchCriteriaGreaterThanShouldReturnObjectsInDatabaseThatAreGreaterThan()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestDecimalProperty = new DecimalSearchCriteria
                    {
                        SearchType = DecimalSearchType.GreaterThan,
                        Value = TestValue
                    }
                };

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDecimalProperty != null && testobject.TestDecimalProperty > TestValue), results.Length);
            }
        }

        [TestMethod]
        public async Task DecimalSearchCriteriaGreaterThanOrEqualsShouldReturnObjectsInDatabaseThatAreGreaterThanOrEqual()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestDecimalProperty = new DecimalSearchCriteria
                    {
                        SearchType = DecimalSearchType.GreaterThanOrEquals,
                        Value = TestValue
                    }
                };

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDecimalProperty != null && testobject.TestDecimalProperty >= TestValue), results.Length);
            }
        }

        [TestMethod]
        public async Task DecimalSearchCriteriaDoesNotEqualShouldReturnObjectsInDatabaseThatDoNotEqual()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestDecimalProperty = new DecimalSearchCriteria
                    {
                        SearchType = DecimalSearchType.DoesNotEqual,
                        Value = TestValue
                    }
                };

                var results = await dbContext.TestObjects.Search(searchCriteria).ToArrayAsync();

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDecimalProperty != null && testobject.TestDecimalProperty != TestValue), results.Length);
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
