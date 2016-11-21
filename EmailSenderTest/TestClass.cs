using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendEmail;

namespace EmailSenderTest
{
    [TestFixture]
    public class TestClass
    {
        Notification nm = new Notification();
        [Test]
        public void getConfigurationTest()
        {
            nm.getConfiguration();
            CollectionAssert.AllItemsAreNotNull(nm.hora_diario);
            // TODO: Add your test code here
            //Assert.Pass("Your first passing test");
        }
    }
}
