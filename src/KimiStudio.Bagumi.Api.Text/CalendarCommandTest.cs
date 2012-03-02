﻿using System;
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
    public class CalendarCommandTest
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
            var command = new CalendarCommand(auth);
            List<Calendar> calendars = command.Execute();


            Assert.That(calendars, Is.Not.Null);
            Assert.That(calendars.Count, Is.EqualTo(7));

            var first = calendars.FirstOrDefault();
            Assert.That(first, Is.Not.Null);
            var visitor = new JsonDateTestVisitor();
            visitor.Visit(calendars);

        }
    }
}
