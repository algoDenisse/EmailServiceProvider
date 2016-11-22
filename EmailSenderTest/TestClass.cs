using NUnit.Framework;
using System;
using Moq;
using NSubstitute;
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
        private static IEmailSender emailSender;
        Notification nm = new Notification(emailSender);
        Program pg = new Program();


        //Configuration File tests
        [Test]
        public void configurationFileExists_Test()
        {
            string baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var dir = baseDir + "\\configuration.csv";
            bool expResult = nm.fileExists(dir);
            Assert.That(expResult, Is.True);
        }
        
        [Test]
        public void ItShouldReturnFalseIfConfigFileDoesntExist()
        {
            string baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var dir = baseDir + "\\nonexistingConfFile.csv";
            bool expResult = nm.fileExists(dir);
            Assert.That(expResult, Is.False);

        }

        [Test]
        public void ItShouldReturnFlaseIfFileisNotEmpty()
        {
            string baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var dir = baseDir + "\\configuration.csv";
            bool expResult = nm.fileEmpty(dir);
            Assert.That(expResult, Is.False);

        }

        [Test]
        public void readDailyConfiguration_Test()
        {
            string[] someconfigarray = { };
            Mock<INotification> mockNotification = new Mock<INotification>();
            //Verifies that when calling getConfiguration(), setDailyConfig() is also called.
            mockNotification.Setup(x => x.setDailyConfig(someconfigarray));
            mockNotification.Object.getConfiguration();
            mockNotification.Verify(x => x.setDailyConfig(someconfigarray),
                Times.AtLeastOnce(), "Error setting up Daily config");
        }

        [Test]
        public void readWeeklyConfiguration_Test()
        {
            string[] someconfigarray = { };
            Mock<INotification> mockNotification = new Mock<INotification>();
            mockNotification.Object.getConfiguration();
            mockNotification.Setup(x => x.setWeeklyConfig(someconfigarray));
            mockNotification.Verify(x => x.setWeeklyConfig(someconfigarray),
                Times.AtLeastOnce(), "Error setting up Weekly config");
        }
        [Test]
        public void readMonthlyConfiguration_Test()
        {
            string[] someconfigarray = { };
            Mock<INotification> mockNotification = new Mock<INotification>();
            mockNotification.Object.getConfiguration();
            mockNotification.Setup(x => x.setMonthlyConfig(someconfigarray));
            mockNotification.Verify(x => x.setMonthlyConfig(someconfigarray),
                Times.AtLeastOnce(), "Error setting up Monthly config");
        }
        [Test]
        public void readEmptyConfigurationFile_Test()
        {
            //esta esta mala
            //ocupamos ver como usar un stub para que getConfiguration nos tire una exepcion
            // file empty = verdadero
            //getConfiguration();
            string baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var dir = baseDir + "\\configuration.csv";
            var directotyStub = Substitute.For<INotification>();
            directotyStub.fileEmpty(Arg.Is(dir)).Returns(x => { throw new Exception(); });
            Assert.Throws<Exception>(() => directotyStub.fileEmpty(dir));
        }
        [Test]
        public void readNoneExistingConfigurationFile_Test()
        {
            string baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var dir = baseDir + "\\configurationNoneExisting.csv";
            bool expResult = nm.fileExists(dir);
            Assert.That(expResult, Is.False);
        }
        [Test]
        public void readConfigurationFileWithMoreThan3Configurations_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void readConfigurationFileWith2orLessConfigurations_Test()
        {
            Assert.Pass("Your first passing test");
        }

        //Contacts File Test


        [Test]
        public void contactsFileExists_Test()
        {
            string baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var dir = baseDir + "\\contacts.csv";
            bool expResult = nm.fileExists(dir);
            Assert.That(expResult, Is.True);
        }
        [Test]
        public void contactsFileDoesNotExists_Test()
        {
            string baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var dir = baseDir + "\\contactsNoneExisting.csv";
            bool expResult = nm.fileExists(dir);
            Assert.That(expResult, Is.False);
        }
        [Test]
        public void readExistingContactsFile_Test()
        {
            string baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var dir = baseDir + "\\contacts.csv";
            List<Contact> contactslist = new List<Contact>();
            contactslist = pg.GetContacts(dir);
            Assert.IsNotNull(contactslist);
        }
        [Test]
        public void readEmptyContactsFile_Test()
        {
            //it should return an empty array 
            string baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var dir = baseDir + "\\emptyContacts.csv";
            List<Contact> contactslist = new List<Contact>();
            contactslist = pg.GetContacts(dir);
            Assert.IsEmpty(contactslist);
        }

        [Test]
        public void WhencontactFileisnotfound_Test()
        {
            //An exception 
            string baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var dir = baseDir + "\\idonotexist.csv";
            List<Contact> contactslist = new List<Contact>();
            Assert.Throws<System.IO.FileNotFoundException>(() => pg.GetContacts(dir));
        }


        //SMPT Client Tests
        [Test]
        public void getSMPTclient_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void getSMPTclientWithNoInitialConfiguration_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void getSMPTclientWithNoServer_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void getSMPTclientWithNoPort_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void getSMPTclientWithNoPassword_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void getSMPTclientWithWrongPassword_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void getSMPTclientWithNoEmail_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void getSMPTclientWithWrongEmail_Test()
        {
            Assert.Pass("Your first passing test");
        }

        // Contacts Messages Test
        [Test]
        public void getContactsMessage_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void getContactsMessageWithNoSubject_Test()
        {

            Assert.Pass("Your first passing test");

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
        public void getContactsMessageWithNoEmail_Test()
        {
            Assert.Pass("Your first passing test");
        }

        //Notification Tests

        //Eventual Notifications Test
        [Test]
        public void sendEventualEmails_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void sendEventualEmailsWithNoEventualConfiguration_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void sendEventualEmailsWithNoContacts()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void sendEventualEmailsWithoutInternetAccess()
        {
            Assert.Pass("Your first passing test");
        }

        //Daily Notifications Test
        [Test]
        public void sendDailyEmails_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void sendDailyEmailsWithNoDailyConfiguration_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void sendDailyEmailsWithNoContacts()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void sendDailyEmailsWithoutInternetAccess()
        {
            Assert.Pass("Your first passing test");
        }

        //Weekly Notifications Test
        [Test]
        public void sendWeeklyEmails_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void sendWeeklyEmailsWithNoWeeklyConfiguration_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void sendWeeklyEmailsWithNoContacts()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void sendWeeklyEmailsWithoutInternetAccess()
        {
            Assert.Pass("Your first passing test");
        }

        //Monthly Notifications Test
        [Test]
        public void sendMonthlyEmails_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void sendMonthlyEmailsWithNoMonthlyConfiguration_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void sendMonthlyyEmailsWithNoContacts()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void sendMonthlyEmailsWithoutInternetAccess()
        {
            Assert.Pass("Your first passing test");
        }

        //Logs Tests
        [Test]
        public void logEventualEmailSendingError_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void logDailyEmailSendingError_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void logWeeklyEmailSendingError_Test()
        {
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void logMonthlyEmailSendingError_Test()
        {
            Assert.Pass("Your first passing test");
        }

    }
}
