using Mbrcld.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Web.Controllers
{
    [Route("Account")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public sealed class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet("ConfirmEmail", Name = "ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail(
            [FromQuery] string email,
            [FromQuery] string token,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await this.accountService.ConfirmEmailAsync(email, token, cancellationToken);
                if (result.IsFailure)
                {
                    throw new Exception();
                }

                return View();
            }
            catch
            {
                return LocalRedirectPermanent("/");
            }
        }

        [HttpGet("ResetPassword", Name = "ResetPassword")]
        public async Task<ActionResult> ResetPassword(
            [FromQuery] string email,
            [FromQuery] string token,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await this.accountService.VerifyPasswordResetTokenAsync(email, token, cancellationToken);
                if (result.Value == false)
                {
                    throw new Exception();
                }
            }
            catch
            {
                return LocalRedirectPermanent("/");
            }
            
            ViewBag.Email = email;
            ViewBag.Token = token;

            return View();
        }

        [HttpPost("ResetPassword")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(
            [FromForm] string email,
            [FromForm] string newPassword,
            [FromForm] string token,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await this.accountService.ResetPasswordAsync(email, token, newPassword, cancellationToken);
                return RedirectToAction("ResetPasswordResult");
            }
            catch
            {
                return LocalRedirectPermanent("/");
            }
        }

        [HttpGet("ResetPasswordResult")]
        public async Task<ActionResult> ResetPasswordResult()
        {
            return View();
        }

        [HttpGet("ChangeEmailConfirmation", Name = "ChangeEmailConfirmation")]
        public async Task<ActionResult> ChangeEmailConfirmation(
            [FromQuery] string currentEmail,
            [FromQuery] string newEmail,
            [FromQuery] string token)
        {
            ViewBag.CurrentEmail = currentEmail;
            ViewBag.NewEmail = newEmail;
            ViewBag.Token = token;
            return View();
        }

        [HttpPost("ChangeEmailConfirmation")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmailConfirmationPost(
            [FromForm] string currentEmail,
            [FromForm] string newEmail,
            [FromForm] string token,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await this.accountService.ChangeEmailAsync(currentEmail, newEmail, token, cancellationToken);
                return RedirectToAction("ChangeEmailResult");
            }
            catch
            {
                return LocalRedirectPermanent("/");
            }
        }

        [HttpGet("ChangeEmailResult")]
        public async Task<ActionResult> ChangeEmailResult()
        {
            return View();
        }
    }
}
