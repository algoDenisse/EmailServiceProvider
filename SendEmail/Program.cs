using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SendEmail
{
    public class Notification : INotification
    {
        private static string baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        //   private static StreamWriter sw = new StreamWriter($"[Log]{DateTime.Now.ToString("yyyy-MM-dd")}.txt", true);
        public string[] hora_diario = new string[2];
        public string[] dia_hora_semanal = new string[2];
        public string[] dia_hora_mensual = new string[2];
        public string configuration_dir = baseDir + "\\configuration.csv";
        public static string contact_dir = baseDir + "\\contacts.csv";


        IEmailSender emailSender;

        public Notification(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public bool fileExists(string path)
        {
            //Some code to test that the file exists
            return File.Exists(path);
        }

        public bool fileEmpty(string path)
        {
            if (new System.IO.FileInfo(path).Length == 0)
            {
                return true;
            }
            return false;
        }

        public void setDailyConfig(string[] config)
        {
            hora_diario[0] = config[1];
            hora_diario[1] = "";
            
        }
        public void setWeeklyConfig(string[] config)
        {
            dia_hora_semanal[0] = config[1];
            dia_hora_semanal[1] = config[2];
        }
        public void setMonthlyConfig(string[] config)
        {
            dia_hora_mensual[0] = config[1];
            dia_hora_mensual[1] = config[2];
        }


        public void getConfiguration()
        {
            var dir = baseDir + "\\configuration.csv";
            if (fileExists(dir) && !fileEmpty(dir))
            {

                try
                {
                    StreamReader sr = new StreamReader(configuration_dir, Encoding.Default);
                    string line;
                    int line_number = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var config = line.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        switch (line_number)
                        {
                            case 0:
                                setDailyConfig(config);
                                break;
                            case 1:
                                setWeeklyConfig(config);
                                break;
                            case 2:
                                setMonthlyConfig(config);
                                break;

                        }

                        line_number++;

                    }
                    sr.Close();
                }
                catch (Exception e)
                {
                    Log(e.Message + e.TargetSite);
                    throw;
                }
            }
            else
            {
                Console.WriteLine("Archivo de configuracion no existente");
                throw new Exception("Archivo inexistente o vacio");

            }


        }

        public void sendDailyMail(List<Contact> contacts)
        {
            var smtpClient = emailSender.GetSmtpClient();
            if (contacts != null)
            {
                foreach (var contact in contacts)
                {
                    if (contact.Type == "D")
                    {
                        emailSender.SendMail(smtpClient, contact);
                    }

                }

                Log("===== Mensajes Diarios enviados =====");
            }
        }
        public void sendWeeklyMail(List<Contact> contacts)
        {
            var smtpClient = emailSender.GetSmtpClient();
            if (contacts != null)
            {
                foreach (var contact in contacts)
                {
                    if (contact.Type == "W")
                    {
                        emailSender.SendMail(smtpClient, contact);
                    }

                }

                Log("===== Mensajes Semanales enviados =====");
            }
        }

        public void sendMonthlyMail(List<Contact> contacts)
        {
            var smtpClient = emailSender.GetSmtpClient();
            if (contacts != null)
            {
                foreach (var contact in contacts)
                {
                    if (contact.Type == "M")
                    {
                        emailSender.SendMail(smtpClient, contact);
                    }

                }

                Log("===== Mensajes Mensuales enviados =====");
            }
        }

        public void sendEventualMail()
        {
            //Console.WriteLine("--- Servicio de Emails --- ");
            List<Contact> contacts = null;
            try
            {
                contacts = emailSender.GetContacts(contact_dir);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.TargetSite);
            }
            var smtpClient = emailSender.GetSmtpClient();
            if (contacts != null)
            {
                foreach (var contact in contacts)
                {
                    if (contact.Type == "E")
                    {
                        emailSender.SendMail(smtpClient, contact);
                    }
                }

                Log("===== Mensaje enviado =====");
            }

        }
        static void Main(string[] args)
        {
            Console.WriteLine("--- Leyendo configuracion del Servicio de Emails --- ");
            Program emailSender = new Program();
            Notification nm = new Notification(emailSender);
            nm.getConfiguration();
            Console.WriteLine("Configuracion Diaria: " + nm.hora_diario[0]);
            Console.WriteLine("Configuracion Semanal: " + nm.dia_hora_semanal[0] + " a las " + nm.dia_hora_semanal[1]);
            Console.WriteLine("Configuracion Mesual: Los " + nm.dia_hora_mensual[0] + " a las " + nm.dia_hora_mensual[1]);

            Console.WriteLine("Desea enviar un correo electronico? (y/n)");
            var ans = Console.ReadLine();
            if (ans == "y")
            {
                nm.sendEventualMail();
            }
            else
            {
                Console.WriteLine("Se enviaran unicamente los correos programados como eventuales en el archivo de configuracion.");
            }




            Console.ReadLine();
        }
        private static void Log(string logs)
        {
            var date = DateTime.Now.TimeOfDay.ToString();
            Console.WriteLine(date + ' ' + logs);
        }

    }


    public class Program : IEmailSender
    {
        public static string baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        //private static StreamWriter sw = new StreamWriter($"[Log]{DateTime.Now.ToString("yyyy-MM-dd")}.txt", true);

        //    sw.Flush();
        //    sw.Close();

        public string server = ConfigurationManager.AppSettings["server"];
        public string port = ConfigurationManager.AppSettings["port"];
        public string password = ConfigurationManager.AppSettings["password"];
        public string email = ConfigurationManager.AppSettings["email"];


        public SmtpClient GetSmtpClient()
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = server;
                smtpClient.Port = Convert.ToInt32(port);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;

                smtpClient.Credentials = new NetworkCredential(email, password);
                return smtpClient;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }


        }

        public void SendMail(SmtpClient smtpClient, Contact contact)
        {
            try
            {
                var mailMessage = GetMailMessage(contact);
                smtpClient.Send(mailMessage);
                Log($"Mensaje enviado a  {contact.Email}");
            }
            catch (Exception ex)
            {
                Log($"Error al enviar el correo a  {contact.Email}");
                Log(ex.Message);
                Log(ex.StackTrace);
            }
        }

        public MailMessage GetMailMessage(Contact contact)
        {
            try
            {
                string subject = ConfigurationManager.AppSettings["subject"];
                string content = GetContent(contact);
                string email = ConfigurationManager.AppSettings["email"];
                MailMessage mailMessage = new MailMessage(email, contact.Email);
                mailMessage.Subject = subject;
                mailMessage.Body = content;
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.Normal;

                mailMessage.CC.Add("lserrano467@gmail.com");
                mailMessage.CC.Add("tracy_sanchez13@hotmail.com");
                return mailMessage;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

        }

        public string GetContent(Contact contact)
        {
            string content = $"<p align='left' >{contact.Body} </p>";
            return content;
        }

        public List<Contact> GetContacts(string file_dir)
        {
            List<Contact> contacts = new List<Contact>();

            try
            {
                StreamReader sr = new StreamReader(file_dir, Encoding.Default);
                string line;
                while ((line = sr.ReadLine()) != null)
                {

                    line = line.Replace("，", ",");
                    line = line.Replace("；", ",");
                    line = line.Replace(";", ",");
                    line = line.Replace("“", "");
                    line = line.Replace("”", "");
                    line = line.Replace("\"", "");
                    line = line.Replace("'", "");


                    var contact = line.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    int ccnumber = 2;
                    if (contact.Length >= ccnumber && !string.IsNullOrEmpty(contact[0]) && !string.IsNullOrEmpty(contact[1]))
                    {
                        string[] ccArray = new string[3];
                        ccArray[0] = contact[0];
                        contacts.Add(new Contact() { Email = contact[0], Body = contact[1], Type = contact[2] });
                    }
                    else
                    {
                        Log($"Contactos Agregados");
                        return contacts;

                    }
                }
                sr.Close();
                return contacts;
            }
            catch (Exception e)
            {
                Log(e.Message + e.TargetSite);
                throw;
            }

        }



        public void Log(string logs)
        {
            var date = DateTime.Now.TimeOfDay.ToString();
            Console.WriteLine(date + ' ' + logs);
            // sw.WriteLine(date + ' '+logs);
        }
    }
    public class Contact
    {
        public string Email { get; set; }
        public string Body { get; set; }
        public string Type { get; set; }
    }
}
