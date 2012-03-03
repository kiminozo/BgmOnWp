using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KimiStudio.Bagumi.Api.Commands;
using NUnit.Framework;

namespace KimiStudio.Bagumi.Api.Text
{
    [TestFixture]
    public class GetSubjectCommandTest
    {
        [Test]
        public void TestInvoke()
        {
            var command = new GetSubjectCommand(975);
            var result = command.Execute();


            Assert.That(result, Is.Not.Null);


            var visitor = new JsonDateTestVisitor();
            visitor.Visit(result);

        }
    }
}
