using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SendEmail
{
    class Program
    {
        private static string baseDir = "";
        private static string email = "";
        private static string yeartosend = "";
        private static string monthtosend = "";
        private static string cctocom = "n";
        private static StreamWriter sw;

        static void Main(string[] args)
        {
            Console.WriteLine("--- 开始执行 --- ");
            baseDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            email = ConfigurationManager.AppSettings["email"];
            yeartosend = ConfigurationManager.AppSettings["yeartosend"];
            monthtosend = ConfigurationManager.AppSettings["monthtosend"];
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            //FileStream fs = new FileStream($"{baseDir}\\[Log]{date}.txt", FileMode.Create);
            sw = new StreamWriter($"{baseDir}\\[Log]{date}.txt", true);
            Console.WriteLine($"===== 同步发送日志到{baseDir}\\[Log]{date}.txt =====") ;
            
            Log($"即将发送，{yeartosend}年{monthtosend}月 服务月报，回车键确认");
            Console.ReadLine();
            Log("是否抄送kf&jszc?(y/n)");
            cctocom = Console.ReadLine();
            sw.WriteLine(cctocom);
            Log($"开始发送邮件");
            List<Contact> contacts = null;
            try
            {
                contacts = GetContacts();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.TargetSite);
            }
            

            var smtpClient = GetSmtpClient();
            if (contacts == null)
            {
                Console.WriteLine("未读取到信息，请修正");
            }
            foreach (var contact in contacts)
            {
                SendMail(smtpClient, contact);
            }

            Log("===== 执行完成 =====");
            sw.Flush();
            sw.Close();
            //fs.Close();
            Console.WriteLine();
            Console.WriteLine(" 回车关闭程序 ");
            Console.ReadLine();
        }

        private static SmtpClient GetSmtpClient()
        {
            try
            {
                string server = ConfigurationManager.AppSettings["server"];
                string port = ConfigurationManager.AppSettings["port"];
                string password = ConfigurationManager.AppSettings["password"];

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
                Log($"Mensaje enviado a, Email = {contact.Email} O={contact.Organization} OU={contact.OrganizationUnit}");
            }
            catch (Exception ex)
            {
                Log($"Error al enviar el correo, Email = {contact.Email} O={contact.Organization}");
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
                MailMessage mailMessage = new MailMessage(email, contact.Email);
                mailMessage.Subject = contact.Organization + subject;
                mailMessage.Body = content;
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.Normal;
                if (contact.MailCcArray != null)
                {
                    for (int i = 0; i < contact.MailCcArray.Length; i++)
                    {
                        mailMessage.CC.Add(contact.MailCcArray[i].ToString());
                    }
                }
                if (cctocom=="y"||cctocom=="yes"||cctocom=="Y")
                {
                    mailMessage.CC.Add("lserrano467@gmail.com");
                    mailMessage.CC.Add("tracy_sanchez13@hotmail.com");
                }
                
                return mailMessage;

            }
            catch (Exception e)
            {

                throw;
            }

        }

        private static string GetContent(Contact contact)
        {
            string content = $"<p align='left' ><span style='font-size:14.0pt;font-family:华文中宋;color:#003366'>"+
"no se que es esto </p><p align='left'  style='margin-left: 40px'><span style='font-size:14.0pt;font-family:华文中宋;color:#003366'>1 : { yeartosend }2 : { monthtosend }Finalmente。</span><span lang='EN-US'><o:p></o:p></span></span></p><p align='left'  style='margin-left: 40px'><span style='font-size:14.0pt;font-family:华文中宋;color:#003366'>Hola： { contact.Organization }</p><p align='left'  style='margin-left: 40px'><span style='font-size:14.0pt;font-family:华文中宋;color:#003366'>Esto es una prueba de qa： { contact.OrganizationUnit }</p><p align='left'  style='margin-left: 40px'><span style='font-size:14.0pt;font-family:华文中宋;color:#003366'>Hola trei：{ contact.SignedInmonth }</span></p><p align='left'  style='margin-left: 40px'><span style='font-size:14.0pt;font-family:华文中宋;color:#003366'>：<span lang='EN-US'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; { contact.SignedTotal }</span></span></p><p align='left'  style='margin-left: 40px'><span style='font-size:14.0pt;font-family:华文中宋;color:#003366'>algo de un chino：<span lang='EN-US'>&nbsp;&nbsp; { contact.RevokTotal }<o:p></o:p></span></span></p><p align='left'  style='margin-left: 40px'><span style='font-size:14.0pt;font-family:华文中宋;color:#1F497D'>no se ptra vez：<span lang='EN-US'>&nbsp;&nbsp; </span></span><span lang='EN-US' style='font-size:14.0pt;font-family:华文中宋;color:red'>{ contact.RemainTosign }</span></p><p align='left' ><span lang='EN-US' style='font-size:14.0pt;font-family:华文中宋;color:#003366'>&nbsp;</span></p><p align='left' ><span style='font-size:14.0pt;font-family:华文中宋;color:red'>注：Cosas chinas que no entiendo”、“jaja”。</span></p>";
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
                    line = line.Replace(" ", "");

                    var contact = line.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    int ccnumber = 7;//将第7之后的信息加入到抄送列表；
                    if (contact.Length >= ccnumber && !string.IsNullOrEmpty(contact[0]) && !string.IsNullOrEmpty(contact[2]) && !string.IsNullOrEmpty(contact[6]))
                    {
                        string[] ccArray = new string[contact.Length - ccnumber];
                        for (int i = ccnumber; i < contact.Length; i++)
                        {
                            ccArray[i - ccnumber] = contact[i];
                        }
                        contacts.Add(new Contact() { Organization = contact[0], OrganizationUnit = contact[1], SignedInmonth = contact[2], SignedTotal = contact[3], RevokTotal = contact[4], RemainTosign = contact[5], Email = contact[6], MailCcArray = ccArray });
                    }
                    else
                    {
                        Log($"信息异常，请修正");
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

        public class Contact
        {
            public string Organization { get; set; }

            public string OrganizationUnit { get; set; }

            public string SignedInmonth { get; set; }

            public string SignedTotal { get; set; }

            public string RevokTotal { get; set; }

            public string RemainTosign { get; set; }

            public string Email { get; set; }

            public string[] MailCcArray { get; set; }
        }

        private static void Log(string logs)
        {
            var date = DateTime.Now.TimeOfDay.ToString() ;
            Console.WriteLine(date+' '+logs);
            sw.WriteLine(date + ' '+logs);
        }
    }
}
