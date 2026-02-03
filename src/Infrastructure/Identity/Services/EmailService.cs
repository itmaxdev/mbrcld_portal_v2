using Mbrcld.Application.Interfaces;
using Mbrcld.SharedKernel.Result;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Identity.Services
{
    internal sealed class EmailService : IEmailService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<EmailService> logger;

        public EmailService(HttpClient httpClient, ILogger<EmailService> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<Result> SendEmailConfirmationAsync(Guid userId, string link, CancellationToken cancellationToken = default)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(new { ConfirmationLink = link }), Encoding.UTF8, "application/json");
                var response = await this.httpClient.PostAsync($"contacts({userId})/Microsoft.Dynamics.CRM.do_SendEmailConfirmationEmail", content);
                return response.IsSuccessStatusCode ? Result.Success() : Result.Failure(response.ReasonPhrase);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "An error has occurred when sending an email");
                return Result.Failure(e.Message);
            }
        }

        public async Task<Result> SendPasswordResetAsync(Guid userId, string link, CancellationToken cancellationToken = default)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(new { PasswordResetLink = link }), Encoding.UTF8, "application/json");
                var response = await this.httpClient.PostAsync($"contacts({userId})/Microsoft.Dynamics.CRM.do_SendPasswordResetEmail", content);
                return response.IsSuccessStatusCode ? Result.Success() : Result.Failure(response.ReasonPhrase);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "An error has occurred when sending an email");
                return Result.Failure(e.Message);
            }
        }

        public async Task<Result> SendChangeEmailConfirmationAsync(Guid userId, string newEmail, string link, CancellationToken cancellation = default)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(new { RecipientEmail = newEmail, ConfirmationLink = link }), Encoding.UTF8, "application/json");
                var response = await this.httpClient.PostAsync($"contacts({userId})/Microsoft.Dynamics.CRM.do_SendChangeEmailConfirmationEmail", content);
                return response.IsSuccessStatusCode ? Result.Success() : Result.Failure(response.ReasonPhrase);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "An error has occurred when sending an email");
                return Result.Failure(e.Message);
            }
        }
    }
}
