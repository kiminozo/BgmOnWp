using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using KimiStudio.Bagumi.Api.Commands;
using KimiStudio.Bagumi.Api.Models;
using NUnit.Framework;

namespace KimiStudio.Bagumi.Api.Text
{
    [TestFixture]
    public class SubjectStateCommandTest
    {
        private AuthUser auth;

        [TestFixtureSetUp]
        public void SetUp()
        {
            auth = TestHelper.Login();
            Assert.That(auth, Is.Not.Null);
        }

        [Test]
        public void TestInvoke()
        {
            var command = new SubjectStateCommand(975, auth);
            var result = command.Execute();


            Assert.That(result, Is.Not.Null);


            var visitor = new JsonDateTestVisitor();
            visitor.Visit(result);

        }
    }


}
