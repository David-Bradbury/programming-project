using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ProgrammingProject.UnitTests
{
    [TestFixture]
    public abstract class MasterTest
    {

        private TransactionScope scope;

        [SetUp]
        public void Setup()
        {
            scope = new TransactionScope();
            // Need to set up DB Access from here...
        }

        [TearDown]
        public void TearDown()
        {
            scope.Dispose();
            // Need to reverse any DB actions
        }
    }

}
