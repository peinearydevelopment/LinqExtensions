namespace QueryBuilder.EntityFramework
{
    using QueryBuilder.Contracts;

    public class SortCriteria
    {
        public string SortColumnName { get; set; }
        public SortCriteriaBase SortCriteriaBase { get; set; }
    }
}
