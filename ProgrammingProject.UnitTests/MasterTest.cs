using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ProgrammingProject.Data;

namespace ProgrammingProject.UnitTests
{
    [TestFixture]
    public class MasterTest
    {

        private const string ConnectionString_ = "Server=(localdb)\\MSSQLLocalDB;Database=TestLocalEasyWalk;Trusted_Connection=True;MultipleActiveResultSets=true";

        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public MasterTest()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        var seedData = new SeedDataTest(context);
                    }

                    _databaseInitialized = true;
                }
            }

        }

        public EasyWalkContext CreateContext()
            => new EasyWalkContext(
                new DbContextOptionsBuilder<EasyWalkContext>()
                    .UseSqlServer(ConnectionString_)
                    .Options);

    }

}
