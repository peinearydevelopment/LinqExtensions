namespace QueryBuilder.EntityFramework.UnitTests
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using QueryBuilder.Contracts;

    [TestClass]
    public class EfBooleanSearchCriteriaTests
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
        public void BooleanSearchCriteriaNoneShouldReturnAllObjectsInDatabase()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestBooleanProperty = new BooleanSearchCriteria
                    {
                        SearchType = BooleanSearchType.None,
                        Value = false
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Length, result.Results.Count);
            }
        }

        [TestMethod]
        public void BooleanSearchCriteriaEqualsShouldReturnObjectsInDatabaseThatEqualTrue()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestBooleanProperty = new BooleanSearchCriteria
                    {
                        SearchType = BooleanSearchType.Equals,
                        Value = true
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestBooleanProperty != null && testobject.TestBooleanProperty.Value), result.Results.Count);
            }
        }

        [TestMethod]
        public void BooleanSearchCriteriaDoesNotEqualShouldReturnObjectsInDatabaseThatDoNotEqualTrue()
        {
            using (var dbContext = new TestDbContext())
            {
                var searchCriteria = new TestSearchCriteria
                {
                    TestBooleanProperty = new BooleanSearchCriteria
                    {
                        SearchType = BooleanSearchType.DoesNotEqual,
                        Value = true
                    }
                };

                var result = dbContext.TestObjects.Search(dbContext, searchCriteria);

                Assert.AreEqual(TestData.TestObjects.Count(testobject => testobject.TestBooleanProperty != null && !testobject.TestBooleanProperty.Value), result.Results.Count);
            }
        }
    }
}
