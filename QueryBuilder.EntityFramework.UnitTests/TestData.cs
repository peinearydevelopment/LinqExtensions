namespace QueryBuilder.EntityFramework.UnitTests
{
    using System;

    public static class TestData
    {
        public static DateTime DateTimeNow = DateTime.Now;
        public static DateTimeOffset DateTimeOffsetNow = DateTimeOffset.Now;

        public static TestObject[] TestObjects =
        {
            new TestObject { TestStringProperty = "a", TestIntegerProperty = 1, TestDateTimeProperty = DateTimeNow.AddDays(-1), TestDateTimeOffsetProperty = null, TestDecimalProperty = (decimal)2.5 },
            new TestObject { TestStringProperty = "ab", TestIntegerProperty = 2, TestDateTimeProperty = null, TestDateTimeOffsetProperty = DateTimeOffsetNow.AddDays(-1), TestDecimalProperty = (decimal)3.0 },
            new TestObject { TestStringProperty = null, TestIntegerProperty = 3, TestDateTimeProperty = DateTimeNow.AddDays(1), TestDateTimeOffsetProperty = DateTimeOffsetNow.AddDays(1), TestDecimalProperty = (decimal)2.0 },
            new TestObject { TestStringProperty = "aa", TestIntegerProperty = null, TestDateTimeProperty = DateTimeNow, TestDateTimeOffsetProperty = DateTimeOffsetNow, TestDecimalProperty = null }
        };
    }
}
