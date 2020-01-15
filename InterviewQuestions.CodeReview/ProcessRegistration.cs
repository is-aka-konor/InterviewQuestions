using System;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Data.SqlClient;

namespace InterviewQuestions.CodeReview
{
    public class ProcessRegistration
    {
        private string Name;
        private string Familiy;
        private string _email;

        private const string _sqlConnectionString = "localhost";
        private readonly SqlConnection Connection = new SqlConnection(_sqlConnectionString); 
        public ProcessRegistration(
            string name,
            string familyName,
            string email,
            string phoneNumber,
            DateTime DOB)
        {
            Name = name;
            Familiy = familyName;
            this._email = email;
        }

        public bool ValidateEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(this._email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public bool SendEmail(string email)
        {
            try
            {
                var mail = new MailMessage("noreply@debitsuccess.com", email);
                var client = new SmtpClient();
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "smtp.gmail.com";
                mail.Subject = "Please confirm yyour registration";
                mail.Body = "Please replay on this email with any text";
                client.Send(mail);
                return true;
            }catch(Exception)
            {
                return false;
            }
        }

        public void IsNewUserSaved(string name, string familyName, string email, DateTime dateOfBirth)
        {
            var cmdText = $"INSERT INTO dbo.users (FistName, LastName, Email, DOB)" +
                          $"VALUES ('{name}','{familyName}', '{email}','{dateOfBirth.ToString()}')";
            var command = new SqlCommand(cmdText, this.Connection);
            command.ExecuteNonQuery();

            if(ValidateEmail(email))
            {
                SendEmail(email);
            }
        }
    }
}
