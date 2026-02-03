using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Application.Features.ProgramQuestions.Queries;
using Mbrcld.Application.Interfaces;
using Mbrcld.Domain.Entities;
using Mbrcld.Web.Constants;
using Mbrcld.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Mbrcld.Application.Hubs;
using AutoMapper.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Mbrcld.Web.API
{
    [Authorize]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/programs")]
    public class ProgramsController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;
        //private bool content;

        public ProgramsController(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator;
            this.configuration = configuration;
        }

        [HttpGet]
        [Route("{id}/questions")]
        public async Task<ActionResult<IList<ListProgramQuestionByProgramIdViewModel>>> GetProgramQuestions([FromRoute] Guid id)
        {
            var programresults = await this.mediator.Send(new ListProgramQuestionByProgramIdQuery(id));
            return Ok(programresults);
        }

        [HttpGet]
        [Route("user-programs")]
        public async Task<ActionResult<IList<ListProgramByUserModuleViewModel>>> GetUserPrograms()
        {
            var userId = User.GetUserId();
            var programs = await this.mediator.Send(new ListProgramByUserModuleQuery(userId));
            foreach (var program in programs)
            {
                program.PictureUrl = Url.RouteUrl("GetAttachedPicture", new { key = program.Id });
            }
            return Ok(programs);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GetProgramByIdViewModel>> GetProgram([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var program = await this.mediator.Send(new GetProgramByIdQuery(id));
            program.PictureUrl = Url.RouteUrl("GetAttachedPicture", new { key = program.Id });
            return Ok(program);
        }

        [HttpGet]
        [Route("inprogress-programs")]
        public async Task<ActionResult<IList<ListProgramByCohortContactViewModel>>> GetInProgressPrograms()
        {
            var userId = User.GetUserId();
            var programs = await this.mediator.Send(new ListProgramByCohortContactQuery(userId));
            foreach (var program in programs)
            {
                program.PictureUrl = Url.RouteUrl("GetAttachedPicture", new { key = program.Id });
            }
            return Ok(programs);
        }

        [HttpGet]
        [Route("active-program")]
        public async Task<ActionResult<GetActiveProgramViewModel>> GetActiveProgram(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var program = await this.mediator.Send(new GetActiveProgramQuery(userId));
            return Ok(program);
        }

        [HttpGet]
        [Route("suggested-programs")]
        public async Task<ActionResult<IList<ListAlumniAvailableProgramViewModel>>> GetAvailablePrograms()
        {
            var userId = User.GetUserId();
            var programs = await this.mediator.Send(new ListAlumniAvailableProgramQuery(userId));
            foreach (var program in programs)
            {
                program.PictureUrl = Url.RouteUrl("GetAttachedPicture", new { key = program.Id });
            }
            return Ok(programs);
        }

        [HttpGet]
        [Route("graduated-programs")]
        public async Task<ActionResult<IList<ListAlumniGraduatedProgramViewModel>>> GetGraduatedPrograms()
        {
            var userId = User.GetUserId();
            var programs = await this.mediator.Send(new ListAlumniGraduatedProgramQuery(userId));
            foreach (var program in programs)
            {
                program.PictureUrl = Url.RouteUrl("GetAttachedPicture", new { key = program.ProgramId });
            }
            return Ok(programs);
        }

        [HttpGet]
        [Route("all-programs")]
        public async Task<ActionResult<IList<ListAllProgramsViewModel>>> GetAllPrograms()
        {
            var programs = await this.mediator.Send(new ListAllProgramsQuery());
            return Ok(programs);
        }

        [HttpGet]
        [Route("cohort-years")]
        public async Task<ActionResult<IList<int>>> GetCohortYears(string programId)
        {
            var obj = new ListCohortYearsQuery();
            obj.programId = programId;
            var years = await this.mediator.Send(obj);
            return Ok(years);
        }

        [HttpGet]
        [Route("all-active-programs")]
        public async Task<ActionResult<IList<ListActiveProgramsViewModel>>> GetActivePrograms()
        {
            var programs = await this.mediator.Send(new ListActiveProgramsQuery());
            foreach (var program in programs)
            {
                program.PictureUrl = Url.RouteUrl("GetAttachedPicture", new { key = program.Id });
            }
            return Ok(programs);
        }

        [HttpGet]
        [Route("program-details/{id}")]
        public async Task<ActionResult<GetProgramDetailsByIdViewModel>> GetProgramDetails([FromRoute] Guid id)
        {
            var userId = User.GetUserId();
            var program = await this.mediator.Send(new GetProgramDetailsByIdQuery(id, userId));
            program.PictureUrl = Url.RouteUrl("GetAttachedLargePicture", new { key = program.Id });
            return Ok(program);
        }

        [HttpPost,DisableRequestSizeLimit]
        [Route("upload-video")]
        public async Task<IActionResult> Uploads()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
               // string uploadedFile = formCollection["file"];
                string id = formCollection["enrollmentId"];
                var file = formCollection.Files.First();
                string path = configuration.GetSection("Mscrm").GetChildren().Where(x => x.Path == "Mscrm:VideosPath").FirstOrDefault().Value;
                var foldername = Path.Combine(path, id);
                if (!Directory.Exists(foldername))
                {
                    Directory.CreateDirectory(foldername);
                }
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), foldername);
                if (file.Length > 0)
                {
                    // isUploaded = false;
                    //var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fileName = $"{id}{Path.GetExtension(file.FileName)}";
                    var fullPath = Path.Combine(pathToSave, fileName);
                    //var dbPath = Path.Combine(foldername, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var successMessage = "Video uploaded successfully.";
                    return Ok(new { message = successMessage });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }   
        }
        [HttpGet, DisableRequestSizeLimit]
        [Route("check-upload-status/{enrollmentId}")]
        public IActionResult CheckUploadStatus([FromRoute]String enrollmentId)
        {
            string path = configuration.GetSection("Mscrm").GetChildren().Where(x => x.Path == "Mscrm:VideosPath").FirstOrDefault().Value;
            var foldername = Path.Combine(path, enrollmentId);
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), foldername);

            //var fileName = $"{enrollmentId}{Path.GetExtension(file)}";
            //var fullPath = Path.Combine(pathToSave, fileName);

            var isUploaded = Directory.Exists(pathToSave);

            return Ok(new { IsUploaded = isUploaded });
        }
    }
}
 