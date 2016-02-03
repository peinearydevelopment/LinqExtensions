namespace QueryBuilder.EntityFramework.UnitTests
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using QueryBuilder.Contracts;

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
        public void IntegerSearchCriteriaNoneShouldReturnAllObjectsInDatabase()
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

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Length, result.Results.Count);
            }
        }

        [TestMethod]
        public void IntegerSearchCriteriaLessThanShouldReturnObjectsInDatabaseThatAreLessThan()
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

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestIntegerProperty != null && testobject.TestIntegerProperty < TestValue), result.Results.Count);
            }
        }

        [TestMethod]
        public void IntegerSearchCriteriaLessThanOrEqualsShouldReturnObjectsInDatabaseThatAreLessThanOrEquals()
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

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestIntegerProperty != null && testobject.TestIntegerProperty <= TestValue), result.Results.Count);
            }
        }

        [TestMethod]
        public void IntegerSearchCriteriaEqualsShouldReturnObjectsInDatabaseThatAreEqual()
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

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestIntegerProperty != null && testobject.TestIntegerProperty == TestValue), result.Results.Count);
            }
        }

        [TestMethod]
        public void IntegerSearchCriteriaGreaterThanShouldReturnObjectsInDatabaseThatAreGreaterThan()
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

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestIntegerProperty != null && testobject.TestIntegerProperty > TestValue), result.Results.Count);
            }
        }

        [TestMethod]
        public void IntegerSearchCriteriaGreaterThanOrEqualsShouldReturnObjectsInDatabaseThatAreGreaterThanOrEqual()
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

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestIntegerProperty != null && testobject.TestIntegerProperty >= TestValue), result.Results.Count);
            }
        }

        [TestMethod]
        public void IntegerSearchCriteriaDoesNotEqualShouldReturnObjectsInDatabaseThatDoNotEqual()
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

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestIntegerProperty != null && testobject.TestIntegerProperty != TestValue), result.Results.Count);
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
