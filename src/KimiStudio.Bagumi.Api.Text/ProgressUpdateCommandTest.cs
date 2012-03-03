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
    public class ProgressUpdateCommandTest
    {
        private AuthUser auth;

        [TestFixtureSetUp]
        public void SetUp()
        {
            auth = TestHelper.Login();
            Assert.That(auth, Is.Not.Null);
        }

        [Test]
        public void TestWatchedInvoke()
        {
            var id = 1807;
            var command = ProgressUpdateCommand.Watched(id, auth);
            var result = command.Execute();


            Assert.That(result, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(200));

            var visitor = new JsonDateTestVisitor();
            visitor.Visit(result);

        }

        //[TearDown]
        //public void TearDown()
        //{
        //    var id = 1807;
        //    var command = ProgressUpdateCommand.Remove(id, auth);
        //    var result = command.Execute();


        //    Assert.That(result, Is.Not.Null);
        //    Assert.That(result.Code, Is.EqualTo(200));

        //    var visitor = new JsonDateTestVisitor();
        //    visitor.Visit(result);

        //}

        [Test]
        public void TestDropInvoke()
        {
            var id = 1807;
            var command = ProgressUpdateCommand.Drop(id, auth);
            var result = command.Execute();


            Assert.That(result, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(200));

            var visitor = new JsonDateTestVisitor();
            visitor.Visit(result);

        }

        [Test]
        public void TestQueueInvoke()
        {
            var id = 1807;
            var command = ProgressUpdateCommand.Queue(id, auth);
            var result = command.Execute();


            Assert.That(result, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(200));

            var visitor = new JsonDateTestVisitor();
            visitor.Visit(result);

        }


        [Test]
        public void TestWatchEndInvoke()
        {
            var id = 1807;
            var command = ProgressUpdateCommand.Watched(id + 1, new List<int>() { id, id + 1 }.ToList(), auth);
            var result = command.Execute();


            Assert.That(result, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(200));

            var visitor = new JsonDateTestVisitor();
            visitor.Visit(result);

        }

    }
}
