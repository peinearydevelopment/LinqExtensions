namespace QueryBuilder.EntityFramework
{
    using System.Collections.Generic;

    public class ObjectSqlMetadata
    {
        public string TableName { get; set; }
        public string SchemaName { get; set; }
        public string PrimaryKeyName { get; set; }
        public IDictionary<string, string> ObjectPropertyToColumnNameMapper { get; set; }

        public ObjectSqlMetadata()
        {
            ObjectPropertyToColumnNameMapper = new Dictionary<string, string>();
        }
    }
}
