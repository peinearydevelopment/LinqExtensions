using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QueryBuilder.EntityFramework.UnitTests
{
    using System;

    [TestClass]
    public static class TestData
    {
        public static DateTime DateTimeNow = DateTime.Now;
        public static DateTimeOffset DateTimeOffsetNow = DateTimeOffset.Now;

        public static TestObject[] TestObjects =
        {
            new TestObject
            {
                TestStringProperty = "a",
                TestIntegerProperty = 1,
                TestDateTimeProperty = DateTimeNow.AddDays(-1),
                TestDateTimeOffsetProperty = null,
                TestDecimalProperty = (decimal)2.5,
                TestBooleanProperty = null
            },
            new TestObject
            {
                TestStringProperty = "ab",
                TestIntegerProperty = 2,
                TestDateTimeProperty = null,
                TestDateTimeOffsetProperty = DateTimeOffsetNow.AddDays(-1),
                TestDecimalProperty = (decimal)3.0,
                TestBooleanProperty = true
            },
            new TestObject
            {
                TestStringProperty = null,
                TestIntegerProperty = 3,
                TestDateTimeProperty = DateTimeNow.AddDays(1),
                TestDateTimeOffsetProperty = DateTimeOffsetNow.AddDays(1),
                TestDecimalProperty = (decimal)2.0,
                TestBooleanProperty = false
            },
            new TestObject
            {
                TestStringProperty = "aa",
                TestIntegerProperty = null,
                TestDateTimeProperty = DateTimeNow,
                TestDateTimeOffsetProperty = DateTimeOffsetNow,
                TestDecimalProperty = null,
                TestBooleanProperty = false
            }
        };

        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            using (var dbContext = new TestDbContext())
            {
                dbContext.TestObjects.RemoveRange(dbContext.TestObjects.ToArray());
                TestObjects = dbContext.TestObjects.AddRange(TestObjects).ToArray();
                dbContext.SaveChanges();
            }
        }

        [AssemblyCleanup]
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
