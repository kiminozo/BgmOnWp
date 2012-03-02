using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KimiStudio.Bagumi.Api.Commands;
using NUnit.Framework;

namespace KimiStudio.Bagumi.Api.Text
{
    [TestFixture]
    public class LoginCommandTest
    {
        [Test]
        public void TestInvoke()
        {
            var command = new LoginCommand("piova", "piova@live.com");
            var auth = command.Execute();

            Assert.IsFalse(IsHtmlString(auth.AuthEncode));
            var visitor = new JsonDateTestVisitor();
            visitor.Visit(auth);
        }

        private bool IsHtmlString(string value)
        {
            return value.Any(c => c == '/');
        }
    }
}
