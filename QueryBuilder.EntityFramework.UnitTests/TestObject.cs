namespace QueryBuilder.EntityFramework.UnitTests
{
    using System;

    public class TestObject
    {
        public int Id { get; set; }
        public string TestStringProperty { get; set; }
        public int? TestIntegerProperty { get; set; }
        public DateTime? TestDateTimeProperty { get; set; }
        public DateTimeOffset? TestDateTimeOffsetProperty { get; set; }
        public decimal? TestDecimalProperty { get; set; }
    }
}
