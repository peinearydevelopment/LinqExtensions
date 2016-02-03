namespace QueryBuilder.EntityFramework.UnitTests
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Contracts;

    [TestClass]
    public class EfDecimalSearchCriteriaTests
    {
        private const decimal TestValue = (decimal)2.5;

        [TestMethod]
        public void EmptySearchCriteriaObjectShouldReturnAllObjectsInDatabase()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria();
                var result =  dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Length, result.Results.Count);
            }
        }

        [TestMethod]
        public void DecimalSearchCriteriaNoneShouldReturnAllObjectsInDatabase()
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

                var result =  dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Length, result.Results.Count);
            }
        }

        [TestMethod]
        public void DecimalSearchCriteriaLessThanShouldReturnObjectsInDatabaseThatAreLessThan()
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

                var result =  dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDecimalProperty != null && testobject.TestDecimalProperty < TestValue), result.Results.Count);
            }
        }

        [TestMethod]
        public void DecimalSearchCriteriaLessThanOrEqualsShouldReturnObjectsInDatabaseThatAreLessThanOrEquals()
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

                var result =  dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDecimalProperty != null && testobject.TestDecimalProperty <= TestValue), result.Results.Count);
            }
        }

        [TestMethod]
        public void DecimalSearchCriteriaEqualsShouldReturnObjectsInDatabaseThatAreEqual()
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

                var result =  dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDecimalProperty != null && testobject.TestDecimalProperty == TestValue), result.Results.Count);
            }
        }

        [TestMethod]
        public void DecimalSearchCriteriaGreaterThanShouldReturnObjectsInDatabaseThatAreGreaterThan()
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

                var result =  dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDecimalProperty != null && testobject.TestDecimalProperty > TestValue), result.Results.Count);
            }
        }

        [TestMethod]
        public void DecimalSearchCriteriaGreaterThanOrEqualsShouldReturnObjectsInDatabaseThatAreGreaterThanOrEqual()
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

                var result =  dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDecimalProperty != null && testobject.TestDecimalProperty >= TestValue), result.Results.Count);
            }
        }

        [TestMethod]
        public void DecimalSearchCriteriaDoesNotEqualShouldReturnObjectsInDatabaseThatDoNotEqual()
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

                var result =  dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestDecimalProperty != null && testobject.TestDecimalProperty != TestValue), result.Results.Count);
            }
        }
    }
}
