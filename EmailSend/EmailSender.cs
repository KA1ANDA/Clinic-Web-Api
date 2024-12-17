using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClinicWebApi.EmailSend
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }

    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var apiKey = _configuration["Mailgun:ApiKey"];
            var domain = _configuration["Mailgun:Domain"];
            var fromEmail = _configuration["Mailgun:FromEmail"];
            var fromName = _configuration["Mailgun:FromName"];

            var requestUri = $"https://api.mailgun.net/v3/{domain}/messages";

            // Create form content with the necessary parameters for Mailgun
            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("from", $"{fromName} <{fromEmail}>"),
                new KeyValuePair<string, string>("to", toEmail),
                new KeyValuePair<string, string>("subject", subject),
                new KeyValuePair<string, string>("text", message)
            });

            // Create request and set the necessary headers for Basic Authentication
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = formData
            };

            // Add Basic Authentication header
            var byteArray = Encoding.ASCII.GetBytes($"api:{apiKey}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            // Send request and await response
            var response = await _httpClient.SendAsync(request);

            // Check for successful response
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"Email sending failed: {response.ReasonPhrase} - {errorResponse}");
            }
        }
    }
}
