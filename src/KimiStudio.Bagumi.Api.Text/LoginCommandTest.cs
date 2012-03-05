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

        [Test]
        public void TestTask()
        {
            var command = new LoginCommand("piova", "piova@live.com");
            var task = new AsyncTask<AuthUser>(command);
            task.CallBack(p =>
                              {
                                  var v = new JsonDateTestVisitor();
                                  v.Visit(p);
                              });
            task.Execute();
            Thread.Sleep(10000);
        }

        private bool IsHtmlString(string value)
        {
            return value.Any(c => c == '/');
        }
    }
}
