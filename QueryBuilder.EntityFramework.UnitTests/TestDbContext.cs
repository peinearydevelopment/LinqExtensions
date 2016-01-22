namespace QueryBuilder.EntityFramework.UnitTests
{
    using System.Data.Entity;

    public class TestDbContext : DbContext
    {
        public TestDbContext() : base("TestDbContext")
        {
        }

        public DbSet<TestObject> TestObjects { get; set; }
    }
}
