﻿namespace QueryBuilder.EntityFramework.UnitTests
{
    using Contracts;

    public class TestSearchCriteria : PagedSearchCriteria
    {
        public StringSearchCriteria TestStringProperty { get; set; }
        public IntegerSearchCriteria TestIntegerProperty { get; set; }
        public DateTimeSearchCriteria TestDateTimeProperty { get; set; }
        public DateTimeOffsetSearchCriteria TestDateTimeOffsetProperty { get; set; }
        public DecimalSearchCriteria TestDecimalProperty { get; set; }
        public BooleanSearchCriteria TestBooleanProperty { get; set; }
    }
}
