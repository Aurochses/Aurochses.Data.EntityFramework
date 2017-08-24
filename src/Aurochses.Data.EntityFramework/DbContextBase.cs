using System.Data.Entity;

namespace Aurochses.Data.EntityFramework
{
    /// <summary>
    /// Class DbContextBase.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    public class DbContextBase<TContext> : DbContext
        where TContext : DbContext
    {
        static DbContextBase()
        {
            Database.SetInitializer<TContext>(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextBase{TContext}"/> class.
        /// </summary>
        /// <param name="nameOrConnectionString">The name or connection string.</param>
        /// <param name="schemaName">Name of the schema.</param>
        public DbContextBase(string nameOrConnectionString, string schemaName)
            : base(nameOrConnectionString)
        {
            SchemaName = schemaName;
        }

        /// <summary>
        /// Database schema name
        /// </summary>
        public string SchemaName
        {
            get;
        }
    }
}