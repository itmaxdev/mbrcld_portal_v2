using Mbrcld.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Web.Controllers
{
    [Route("References")]
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ReferencesController : Controller
    {
        private readonly IReferenceRepository referenceRepository;

        public ReferencesController(IReferenceRepository referenceRepository)
        {
            this.referenceRepository = referenceRepository;
        }

        [HttpGet("{id}", Name = "RenderReferenceForm")]
        public async Task<ActionResult> References([FromRoute]Guid id, CancellationToken cancellationToken)
        {
            var reference = await this.referenceRepository.GetByIdAsync(id, cancellationToken);
            if (reference.HasNoValue)
            {
                ViewBag.HasNoValue = "This link is not valid or does not exist.";
                return View();
            }
            if (reference.Value.IsCompleted)
            {
                ViewBag.IsCompleted = "Data already submited.";
                return View();
            }
            ViewBag.Id = id.ToString();
            return View();
        }

        [HttpPost("SubmitReferenceForm")]
        public async Task<ActionResult> Post(
            [FromBody] ReferenceSubmitModel model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { HasNoValue = true });
            }

            var reference = await this.referenceRepository.GetByIdAsync(model.Id, cancellationToken);
            if (reference.HasNoValue)
            {
                return Json(new { HasNoValue = true });
            }
            if (reference.Value.IsCompleted)
            {
                return Json(new { HasCompleted = true });
            }

            var referenceValue = reference.Value;

            referenceValue.Strengths = model.Strengths;
            referenceValue.Impact = model.Impact; 
            referenceValue.Competency = model.Competency; 
            referenceValue.Capacity = model.Capacity; 
            referenceValue.AreasOfImprovement = model.AreasOfImprovement;
            referenceValue.AdditionalDetails = model.AdditionalDetails; 
            referenceValue.IsCompleted = true;

            await this.referenceRepository.UpdateAsync(referenceValue, cancellationToken);

            return Json(new { Success = true });
        }
    }

    public sealed class ReferenceSubmitModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Strengths { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Impact { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Competency { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Capacity { get; set; }

        [Required]
        [MaxLength(2000)]
        public string AreasOfImprovement { get; set; }

        [Required]
        [MaxLength(2000)]
        public string AdditionalDetails { get; set; }
    }
}
