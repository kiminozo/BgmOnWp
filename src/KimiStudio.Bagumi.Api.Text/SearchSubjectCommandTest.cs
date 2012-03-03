using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KimiStudio.Bagumi.Api.Commands;
using KimiStudio.Bagumi.Api.Models;
using NUnit.Framework;

namespace KimiStudio.Bagumi.Api.Text
{
    public class SearchSubjectCommandTest
    {
        //private AuthUser auth;

        //[TestFixtureSetUp]
        //public void SetUp()
        //{
        //    auth = TestHelper.Login();
        //    Assert.That(auth, Is.Not.Null);
        //}

        [Test]
        public void TestWatchedInvoke()
        {
            var command = new SearchSubjectCommand("air");
            var result = command.Execute();


            Assert.That(result, Is.Not.Null);

            var visitor = new JsonDateTestVisitor();
            visitor.Visit(result);

        }
    }
}
