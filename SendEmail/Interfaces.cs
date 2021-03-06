﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SendEmail
{
    public interface IEmailSender
    {
        void SendMail(SmtpClient smtpClient, Contact contact);
        SmtpClient GetSmtpClient();
        MailMessage GetMailMessage(Contact contact);
        string GetContent(Contact contact);
        List<Contact> GetContacts(string file_dir);
        void Log(string logs);
    }

    public interface INotification
    {
        void getConfiguration(string dir);
        bool fileExists(string path);
        bool fileEmpty(string path);
        void setDailyConfig(string[] config);
        void setWeeklyConfig(string[] config);
        void setMonthlyConfig(string[] config);
        Boolean sendDailyMail(List<Contact> contacts);
        Boolean sendWeeklyMail(List<Contact> contacts);
        Boolean sendMonthlyMail(List<Contact> contacts);
        Boolean sendEventualMail();
    }
}
