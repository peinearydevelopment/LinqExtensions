namespace QueryBuilder.EntityFramework
{
    using System.Text;

    public class DynamicQuery
    {
        public StringBuilder ParameterizedQuery { get; set; }
        public object[] Parameters { get; set; }
    }
}
