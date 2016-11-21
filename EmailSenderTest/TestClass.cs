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
            string baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            nm.configuration_dir = baseDir + "\\configuration.csv";
            nm.getConfiguration();
            CollectionAssert.AllItemsAreNotNull(nm.hora_diario);
            CollectionAssert.AllItemsAreNotNull(nm.dia_hora_mensual);
            CollectionAssert.AllItemsAreNotNull(nm.dia_hora_semanal);
            // TODO: Add your test code here
            //Assert.Pass("Your first passing test");
        }

        [Test]
        public void FileDoesntExist()
        {
            nm.configuration_dir = "";
            nm.getConfiguration();
            //Assert.Throws
            // TODO: Add your test code here
            //Assert.Pass("Your first passing test");
        }
    }
}
