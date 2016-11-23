using NUnit.Framework;
using System;
using Moq;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendEmail;
using System.Net.Mail;
using System.IO;

namespace EmailSenderTest
{
    [TestFixture]
    public class TestClass
    {
        private static IEmailSender emailSender;
        Notification nm = new Notification(emailSender);
        
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
            mockNotification.Object.getConfiguration(nm.configuration_dir);
            mockNotification.Verify(x => x.setDailyConfig(someconfigarray),
                Times.AtLeastOnce(), "Error setting up Daily config");
        }

        [Test]
        public void readWeeklyConfiguration_Test()
        {
            string[] someconfigarray = { };
            Mock<INotification> mockNotification = new Mock<INotification>();
            mockNotification.Object.getConfiguration(nm.configuration_dir);
            mockNotification.Setup(x => x.setWeeklyConfig(someconfigarray));
            mockNotification.Verify(x => x.setWeeklyConfig(someconfigarray),
                Times.AtLeastOnce(), "Error setting up Weekly config");
        }
        [Test]
        public void readMonthlyConfiguration_Test()
        {
            string[] someconfigarray = { };
            Mock<INotification> mockNotification = new Mock<INotification>();
            mockNotification.Object.getConfiguration(nm.configuration_dir);
            mockNotification.Setup(x => x.setMonthlyConfig(someconfigarray));
            mockNotification.Verify(x => x.setMonthlyConfig(someconfigarray),
                Times.AtLeastOnce(), "Error setting up Monthly config");
        }
        [Test]
        public void readEmptyConfigurationFile_Test()
        {
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
            Program pg = new Program();
            string baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var dir = baseDir + "\\contacts.csv";
            List<Contact> contactslist = new List<Contact>();
            contactslist = pg.GetContacts(dir);
            Assert.IsNotNull(contactslist);
        }
        [Test]
        public void readEmptyContactsFile_Test()
        {
            Program pg = new Program();
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
            Program pg = new Program();
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
            Program pg = new Program();
            SmtpClient expectedResult = pg.GetSmtpClient();
            Assert.IsInstanceOf<SmtpClient>(expectedResult);
        }
        [Test]
        public void getSMPTclientWithNoInitialConfiguration_Test()
        {
            Program pg = new Program();
            pg.server = null;
            pg.port = null;
            pg.email = null;
            pg.password = null;
            string expectedResult;
            try
            {
                SmtpClient client = pg.GetSmtpClient();
            }
            catch (Exception e)
            {
                expectedResult = e.Message;
            }
            Assert.Throws<System.ArgumentNullException>(() => pg.GetSmtpClient());

        }
        [Test]
        public void getSMPTclientWithNoServer_Test()
        {
            Program pg = new Program();
            pg.server = null;
            string expectedResult;
            try
            {
                SmtpClient client = pg.GetSmtpClient();
            }
            catch (Exception e)
            {
                expectedResult = e.Message;
            }
            Assert.Throws<System.ArgumentNullException>(() => pg.GetSmtpClient());
        }
        [Test]
        public void getSMPTclientWithNoPort_Test()
        {
            Program pg = new Program();
            pg.port = null;
            string expectedResult = "";
            try
            {
                SmtpClient client = pg.GetSmtpClient();
            }
            catch (Exception e)
            {
                expectedResult = e.Message;
            }
            
           StringAssert.Contains("El argumento especificado está fuera del intervalo de valores válidos", expectedResult);

        }
        [Test]
        public void sendMailWithNoPassword_Test()
        {
            Program pg = new Program();
            pg.password = null;
            string expectedResult;
            SmtpClient client = pg.GetSmtpClient();
            MailMessage mailMessage = new MailMessage("lserrano467@gmail.com", "denissepaolarojas@hotmail.com");
            try
            {
                client.Send(mailMessage);
            }
            catch (Exception e)
            {
                expectedResult = e.Message;

            }
            Assert.Throws<System.Net.Mail.SmtpException>(() => client.Send(mailMessage));
        }
        [Test]
        public void sendMailtWithWrongPassword_Test()
        {
            Program pg = new Program();
            pg.password = "1234";
            string expectedResult ="";
            SmtpClient client = pg.GetSmtpClient();
            MailMessage mailMessage = new MailMessage("lserrano467@gmail.com", "denissepaolarojas@hotmail.com");
            try
            {
                client.Send(mailMessage);
            }
            catch (Exception e)
            {
                expectedResult = e.Message;
             }
            StringAssert.Contains("El servidor SMTP requiere una conexión segura o el cliente no se autenticó", expectedResult);
        }
        [Test]
        public void sendMailWithNoEmail_Test()
        {
            Program pg = new Program();
            pg.email = null;
            string expectedResult = "";
            SmtpClient client = pg.GetSmtpClient();
            try
            {
                MailMessage mailMessage = new MailMessage(pg.email, "denissepaolarojas@hotmail.com");
                client.Send(mailMessage);
            }
            catch (Exception e)
            {
                expectedResult = e.Message;
            }
            StringAssert.AreEqualIgnoringCase(expectedResult, "El valor no puede ser nulo.\r\nNombre del parámetro: from");           
        }
        [Test]
        public void sendMailWithWrongEmail_Test()
        {
            Program pg = new Program();
            pg.email = "lserrano47@gmail.com";
            string expectedResult = "";
            SmtpClient client = pg.GetSmtpClient();
            MailMessage mailMessage = new MailMessage(pg.email, "denissepaolarojas@hotmail.com");
            try
            {
                client.Send(mailMessage);
            }
            catch (Exception e)
            {
                expectedResult = e.Message;
            }
            StringAssert.Contains("El servidor SMTP requiere una conexión segura o el cliente no se autenticó", expectedResult);
        }

        // Contacts Messages Test
        [Test]
        public void getContactsMessage_Test()
        {
            Program pg = new Program();
            SmtpClient client = pg.GetSmtpClient();
            Contact contact = new Contact();
            contact.Email = "denissepaolarojas@hotmail.com";
            contact.Body = "Hola Denisse";
            contact.Type = "D";
            string expectedResult = "";
            MailMessage mm = new MailMessage();

            try
            {
                mm = pg.GetMailMessage(contact);
            }
            catch(Exception e)
            {
                expectedResult = e.Message;
            }
            StringAssert.AreEqualIgnoringCase($"<p align='left' >Hola Denisse </p>", mm.Body);
        }

        //Notification Tests

        //Eventual Notifications Test
        [Test]
        public void sendEventualEmails_Test()
        {
            var stub = Substitute.For<INotification>();
            stub.sendEventualMail().Returns(x => true);
            var result = stub.sendEventualMail();
            Assert.That(true, Is.EqualTo(result));
        }
        
        //Daily Notifications Test
        [Test]
        public void sendDailyEmails_Test()
        {
            Contact contact = new Contact();
            List<Contact> contacts = new List<Contact>();
            contacts.Add(contact); 

            var stub = Substitute.For<INotification>();
            stub.sendDailyMail(contacts).Returns(x => true);
            var result = stub.sendDailyMail(contacts);
            Assert.That(true, Is.EqualTo(result));
        }
              //Weekly Notifications Test
        [Test]
        public void sendWeeklyEmails_Test()
        {
            Contact contact = new Contact();
            List<Contact> contacts = new List<Contact>();
            contacts.Add(contact);

            var stub = Substitute.For<INotification>();
            stub.sendWeeklyMail(contacts).Returns(x => true);
            var result = stub.sendWeeklyMail(contacts);
            Assert.That(true, Is.EqualTo(result));
        }
     
        //Monthly Notifications Test
        [Test]
        public void sendMonthlyEmails_Test()
        {
            Contact contact = new Contact();
            List<Contact> contacts = new List<Contact>();
            contacts.Add(contact);

            var stub = Substitute.For<INotification>();
            stub.sendMonthlyMail(contacts).Returns(x => true);
            var result = stub.sendMonthlyMail(contacts);
            Assert.That(true, Is.EqualTo(result));
        }
        
        //Logs Tests
        [Test]
        public void ShouldCreateLogFileIfEmailisnotSent_Test()
        {
            string baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            Program pg = new Program();
            pg.password = "1234";
            SmtpClient client = pg.GetSmtpClient();
            MailMessage mailMessage = new MailMessage("lserrano467@gmail.com", "denissepaolarojas@hotmail.com");

            try
            {
                client.Send(mailMessage);
            }
            catch (Exception e)
            {
                Assert.IsTrue(File.Exists(baseDir + "\\Log.txt"));
            }

        }

    }
}
