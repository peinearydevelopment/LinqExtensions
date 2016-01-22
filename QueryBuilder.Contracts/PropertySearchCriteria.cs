namespace QueryBuilder.Contracts
{
    public class PropertySearchCriteria<T, TSearchType>
    {
        public TSearchType SearchType { get; set; }
        public T Value { get; set; }
        public SortType SortType { get; set; }
        public int SortOrder { get; set; }
    }
}
