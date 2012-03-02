using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KimiStudio.Bagumi.Api.Commands;
using KimiStudio.Bagumi.Api.Models;
using NUnit.Framework;

namespace KimiStudio.Bagumi.Api.Text
{
    [TestFixture]
    public class ProgressCommandTest
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
            var id = 9779;
            var command = new ProgressCommand(id, auth);
            var result = command.Execute();


            Assert.That(result, Is.Not.Null);
            Assert.That(result.SubjectId, Is.EqualTo(id));

            var visitor = new JsonDateTestVisitor();
            visitor.Visit(result);

        }

    }
}
