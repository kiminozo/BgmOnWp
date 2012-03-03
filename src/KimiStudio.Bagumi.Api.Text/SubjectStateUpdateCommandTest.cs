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
    public class SubjectStateUpdateCommandTest
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
            var command = new SubjectStateUpdateCommand(new SubjectStateUpdateInfo
                                                            {
                                                                Comment = "以为是暴力，结果比较搞笑",
                                                                Method = "do",
                                                                Rating = 7,
                                                                SubjectId = 9779,
                                                                Tags = new[] { "搞笑暴力", "恶魔奶爸 " }
                                                            }, auth);
            var result = command.Execute();


            Assert.That(result, Is.Not.Null);


            var visitor = new JsonDateTestVisitor();
            visitor.Visit(result);

        }
    }
}
