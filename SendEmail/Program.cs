using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SendEmail
{
    public class Notification
    {
        private static string baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        private static StreamWriter sw = new StreamWriter($"[Log]{DateTime.Now.ToString("yyyy-MM-dd")}.txt", true);
        public string[] hora_diario = new string[2];
        public string[] dia_hora_semanal = new string[2];
        public string[] dia_hora_mensual = new string[2];

        public Notification()
        {

        }
        public void getConfiguration()
        {
            var dir = baseDir + "\\configuration.csv";
            try
            {
                StreamReader sr = new StreamReader(dir, Encoding.Default);
                string line;
                int line_number = 0;
                while ((line = sr.ReadLine()) != null)
                {
                   var config = line.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                   switch (line_number){
                        case 0:
                            hora_diario[0] = config[1];
                            hora_diario[1] = "";
                            break;
                        case 1:
                            dia_hora_semanal[0] = config[1];
                            dia_hora_semanal[1] = config[2];
                            break;
                        case 2:
                            dia_hora_mensual[0] = config[1];
                            dia_hora_mensual[1] = config[2];
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

        public void sendDailyMail()
        {

        }
        public void sendWeeklyMail()
        {

        }
        
        public void sendMonthlyMail()
        {

        }

        public void sendMail()
        {
            Console.WriteLine("--- Servicio de Emails --- ");
            List<Contact> contacts = null;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("--- Leyendo configuracion del Servicio de Emails --- ");
            Notification nm = new Notification();
            nm.getConfiguration();
            Console.WriteLine("Configuracion Diaria: " + nm.hora_diario[0]);
            Console.WriteLine("Configuracion Semanal: " + nm.dia_hora_semanal[0] +" a las "+ nm.dia_hora_semanal[1]);
            Console.WriteLine("Configuracion Mesual: Los " + nm.dia_hora_mensual[0]+ " a las " + nm.dia_hora_mensual[1]);

            Console.WriteLine("Desea enviar un correo electronico? (y/n)");




            Console.ReadLine();
        }
        private static void Log(string logs)
        {
            var date = DateTime.Now.TimeOfDay.ToString();
            Console.WriteLine(date + ' ' + logs);
            sw.WriteLine(date + ' ' + logs);
        }

    }

    public class Program
    {
        private static string baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        private static StreamWriter sw = new StreamWriter($"[Log]{DateTime.Now.ToString("yyyy-MM-dd")}.txt", true);

        //static void Main(string[] args)
        //{
        //    Console.WriteLine("--- Servicio de Emails --- ");
        //    baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        //    var date = DateTime.Now.ToString("yyyy-MM-dd");
        //    sw = new StreamWriter($"[Log]{date}.txt", true);
        //    Console.WriteLine($"=====[Log]{date}.txt =====") ;
           
        //    List<Contact> contacts = null;
        //    try
        //    {
        //        contacts = GetContacts();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        Console.WriteLine(e.TargetSite);
        //    }
            

        //    var smtpClient = GetSmtpClient();
        //    if (contacts != null)
        //    {
        //        foreach (var contact in contacts)
        //        {
        //            SendMail(smtpClient, contact);
        //        }

        //        Log("===== Mensaje enviado =====");
        //    }
            
        //    sw.Flush();
        //    sw.Close();
        //    Console.WriteLine();
        //    Console.WriteLine(" Mensaje Enviado ");
        //    Console.ReadLine();
        //}

        private static SmtpClient GetSmtpClient()
        {
            try
            {
                string server = ConfigurationManager.AppSettings["server"];
                string port = ConfigurationManager.AppSettings["port"];
                string password = ConfigurationManager.AppSettings["password"];
                string email = ConfigurationManager.AppSettings["email"];

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

        private static void SendMail(SmtpClient smtpClient, Contact contact)
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

        private static MailMessage GetMailMessage(Contact contact)
        {
            try
            {
                string subject = ConfigurationManager.AppSettings["subject"];
                string content = GetContent(contact);
                string email = ConfigurationManager.AppSettings["email"];
                MailMessage mailMessage = new MailMessage(email, contact.Email);
                mailMessage.Subject =  subject;
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

                throw;
            }

        }

        private static string GetContent(Contact contact)
        {
            string content = $"<p align='left' >{contact.Body} </p>";  
            return content;
        }

        private static List<Contact> GetContacts()
        {
            List<Contact> contacts = new List<Contact>();
            var dir = baseDir + "\\contacts.csv";
            try
            {
                StreamReader sr = new StreamReader(dir, Encoding.Default);
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
                        contacts.Add(new Contact() {  Email = contact[0], Body = contact[1] });
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
                Log(e.Message+e.TargetSite);
                throw;
            }

        }

       

        private static void Log(string logs)
        {
            var date = DateTime.Now.TimeOfDay.ToString() ;
            Console.WriteLine(date+' '+logs);
            sw.WriteLine(date + ' '+logs);
        }
    }
    public class Contact
    {
        public string Email { get; set; }
        public string Body { get; set; }
    }
}
