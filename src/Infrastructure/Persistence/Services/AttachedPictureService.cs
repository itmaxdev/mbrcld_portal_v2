using Mbrcld.Application.Interfaces;
using Mbrcld.Infrastructure.Configuration;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.Infrastructure.Persistence.Models;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Services
{
    internal sealed class AttachedPictureService : IAttachedPictureService
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IConfiguration configuration;

        public AttachedPictureService(ISimpleWebApiClient webApiClient, IConfiguration configuration)
        {
            this.webApiClient = webApiClient;
            this.configuration = configuration;
        }

        public async Task<Maybe<byte[]>> GetAttachedPictureAsync(Guid key, CancellationToken cancellationToken = default)
        {
            var model = await this.webApiClient.For<ODataAnnotation>()
                .Filter(x => x.Regarding == key)
               // .Filter(x => !x.Subject.StartsWith("Large-"))
                .Top(1)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectToModel()
                .FindEntryAsync();

            return this.IsArrayNullOrEmpty(model?.DocumentBody)
                ? Maybe<byte[]>.None
                : Convert.FromBase64String(model?.DocumentBody);
        }

        public async Task<Maybe<byte[]>> GetAttachedFileAsync(Guid key, CancellationToken cancellationToken = default)
        {
            var model = await this.webApiClient.For<ODataAnnotation>()
                .Filter(x => x.Regarding == key)
                .Top(1)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectToModel()
                .FindEntryAsync();

            return this.IsArrayNullOrEmpty(model?.DocumentBody)
                ? Maybe<byte[]>.None
                : Convert.FromBase64String(model?.DocumentBody);
        }

        public async Task<Maybe<byte[]>> GetAttachedLargePictureAsync(Guid key, CancellationToken cancellationToken = default)
        {
            var model = await this.webApiClient.For<ODataAnnotation>()
                .Filter(x => x.Regarding == key)
                .Filter(x => x.FileName.StartsWith("Large-"))
                .Top(1)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectToModel()
                .FindEntryAsync();

            return this.IsArrayNullOrEmpty(model?.DocumentBody)
                ? Maybe<byte[]>.None
                : Convert.FromBase64String(model?.DocumentBody);
        }

        public async Task<Maybe<byte[]>> GetAttachedCVAsync(Guid key, CancellationToken cancellationToken = default)
        {
            var model = await this.webApiClient.For<ODataAnnotation>()
                .Filter(x => x.Id == key)
                .Top(1)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectToModel()
                .FindEntryAsync();

            return this.IsArrayNullOrEmpty(model?.DocumentBody)
                ? Maybe<byte[]>.None
                : Convert.FromBase64String(model?.DocumentBody);
        }

        public async Task<List<string>> GetAttachedCVTypeAsync(Guid key, CancellationToken cancellationToken = default)
        {
            var model = await this.webApiClient.For<ODataAnnotation>()
                .Filter(x => x.Id == key)
                .Top(1)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectToModel()
                .FindEntryAsync();

            if (model != null)
            {
                var fileName = model.FileName;
                var fileType = model.MimeType;
                List<string> file = new List<string>();
                file.Add(fileName);
                file.Add(fileType);

                return this.IsArrayNullOrEmpty(model?.DocumentBody)
                    ? null
                    : file;
            }
            return null;
        }

        public async Task<Maybe<byte[]>> GetAttachedSmallPictureAsync(Guid key, CancellationToken cancellationToken = default)
        {
            var model = await this.webApiClient.For<ODataAnnotation>()
                .Filter(x => x.Regarding == key)
                .Filter(x => x.FileName.StartsWith("Small-"))
                .Top(1)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectToModel()
                .FindEntryAsync();

            return this.IsArrayNullOrEmpty(model?.DocumentBody)
                ? Maybe<byte[]>.None
                : Convert.FromBase64String(model?.DocumentBody);
        }

        public async Task<List<string>> GetAttachedFileTypeAsync(Guid key, CancellationToken cancellationToken = default)
        {
            var model = await this.webApiClient.For<ODataAnnotation>()
                .Filter(x => x.Regarding == key)
                .Top(1)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectToModel()
                .FindEntryAsync();

            if (model != null)
            {
                var fileName = model.FileName;
                var fileType = model.MimeType;
                List<string> file = new List<string>();
                file.Add(fileName);
                file.Add(fileType);

                return this.IsArrayNullOrEmpty(model?.DocumentBody)
                    ? null
                    : file;
            }
            return null;
        }

        private bool IsArrayNullOrEmpty(string array)
        {
            return array == null || array.Length == 0;
        }

        public async Task<Result> AddOrEditArticlePictureAsync(
            Guid articleid,
            byte[] content,
            string contentType,
            string fileName,
            CancellationToken cancellationToken = default
            )
        {
            var existing = await this.webApiClient.For<ODataAnnotation>()
                .Top(1)
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => x.Id)
                .Filter(x => x.Regarding == articleid)
                .FindEntriesAsync();

            if (existing.Any())
            {
                var id = (Guid)existing.First().Id;

                await this.webApiClient.For("annotations")
                    .Key(id)
                    .Set(new
                    {
                        filename = fileName,
                        mimetype = contentType,
                        documentbody = Convert.ToBase64String(content),
                    })
                    .UpdateEntryAsync(false)
                    .ConfigureAwait(false);
            }
            else
            {
                if (content != null)
                {
                    await this.webApiClient.For<ODataAnnotation>()
                    .Set(new
                    {
                        FileName = fileName,
                        MimeType = contentType,
                        DocumentBody = Convert.ToBase64String(content),
                        objectid_do_article = new { do_articleid = articleid }
                    })
                    .InsertEntryAsync(false)
                    .ConfigureAwait(false);
                }
            }

            return Result.Success();
        }

        public async Task<Result> AddOrEditProjectIdeaFileAsync(
            Guid projectIdeaId,
            byte[] content,
            string contentType,
            string fileName,
            CancellationToken cancellationToken = default
            )
        {
            var existing = await this.webApiClient.For<ODataAnnotation>()
                .Top(1)
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => x.Id)
                .Filter(x => x.Regarding == projectIdeaId)
                .FindEntriesAsync();

            if (existing.Any())
            {
                var id = (Guid)existing.First().Id;

                await this.webApiClient.For("annotations")
                    .Key(id)
                    .Set(new
                    {
                        filename = fileName,
                        mimetype = contentType,
                        documentbody = Convert.ToBase64String(content),
                    })
                    .UpdateEntryAsync(false)
                    .ConfigureAwait(false);
            }
            else
            {
                if (content != null)
                {
                    await this.webApiClient.For<ODataAnnotation>()
                    .Set(new
                    {
                        FileName = fileName,
                        MimeType = contentType,
                        DocumentBody = Convert.ToBase64String(content),
                        objectid_do_projectidea = new { do_projectideaid = projectIdeaId }
                    })
                    .InsertEntryAsync(false)
                    .ConfigureAwait(false);
                }
            }

            return Result.Success();
        }

        public async Task<Result> AddOrEditSpecialProjectFileAsync(
            Guid specialProjectId,
            byte[] content,
            string contentType,
            string fileName,
            CancellationToken cancellationToken = default
            )
        {
            var existing = await this.webApiClient.For<ODataAnnotation>()
                .Top(1)
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => x.Id)
                .Filter(x => x.Regarding == specialProjectId)
                .FindEntriesAsync();

            if (existing.Any())
            {
                var id = (Guid)existing.First().Id;

                await this.webApiClient.For("annotations")
                    .Key(id)
                    .Set(new
                    {
                        filename = fileName,
                        mimetype = contentType,
                        documentbody = Convert.ToBase64String(content),
                    })
                    .UpdateEntryAsync(false)
                    .ConfigureAwait(false);
            }
            else
            {
                if (content != null)
                {
                    await this.webApiClient.For<ODataAnnotation>()
                    .Set(new
                    {
                        FileName = fileName,
                        MimeType = contentType,
                        DocumentBody = Convert.ToBase64String(content),
                        objectid_do_specialproject = new { do_specialprojectid = specialProjectId }
                    })
                    .InsertEntryAsync(false)
                    .ConfigureAwait(false);
                }
            }

            return Result.Success();
        }

        public async Task<Result> AddOrEditProgramContentPictureAsync(
            Guid contentid,
            byte[] content,
            string contentType,
            string fileName,
            CancellationToken cancellationToken = default
            )
        {
            var existing = await this.webApiClient.For<ODataAnnotation>()
                .Top(1)
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => x.Id)
                .Filter(x => x.Regarding == contentid)
                .FindEntriesAsync();

            if (existing.Any())
            {
                var id = (Guid)existing.First().Id;

                await this.webApiClient.For("annotations")
                    .Key(id)
                    .Set(new
                    {
                        filename = fileName,
                        mimetype = contentType,
                        documentbody = Convert.ToBase64String(content),
                    })
                    .UpdateEntryAsync(false)
                    .ConfigureAwait(false);
            }
            else
            {
                if (content != null)
                {
                    await this.webApiClient.For<ODataAnnotation>()
                    .Set(new
                    {
                        FileName = fileName,
                        MimeType = contentType,
                        DocumentBody = Convert.ToBase64String(content),
                        objectid_do_sectioncontent = new { do_sectioncontentid = contentid }
                    })
                    .InsertEntryAsync(false)
                    .ConfigureAwait(false);
                }
            }

            return Result.Success();
        }

        public async Task<Result> AddOrEditNewsfeedDocumentAsync(
            Guid newsfeedid,
            byte[] content,
            string contentType,
            string fileName,
            CancellationToken cancellationToken = default
            )
        {
            var existing = await this.webApiClient.For<ODataAnnotation>()
                .Top(1)
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => x.Id)
                .Filter(x => x.Regarding == newsfeedid)
                .FindEntriesAsync();

            if (existing.Any())
            {
                var id = (Guid)existing.First().Id;

                await this.webApiClient.For("annotations")
                    .Key(id)
                    .Set(new
                    {
                        filename = fileName,
                        mimetype = contentType,
                        documentbody = Convert.ToBase64String(content),
                    })
                    .UpdateEntryAsync(false)
                    .ConfigureAwait(false);
            }
            else
            {
                if (content != null)
                {
                    await this.webApiClient.For<ODataAnnotation>()
                    .Set(new
                    {
                        FileName = fileName,
                        MimeType = contentType,
                        DocumentBody = Convert.ToBase64String(content),
                        objectid_do_newsfeed = new { do_newsfeedid = newsfeedid }
                    })
                    .InsertEntryAsync(false)
                    .ConfigureAwait(false);
                }
            }

            return Result.Success();
        }

        public async Task<Result> RemoveAttachedDocumentAsync(Guid Id, CancellationToken cancellationToken = default)
        {
            await this.webApiClient.For<ODataAnnotation>()
                    .Filter(x => x.Regarding == Id)
                    .DeleteEntriesAsync();

            return Result.Success();
        }

        public async Task<Result> AddAttachmentAsync(
            Guid projectid,
            byte[] content,
            string contentType,
            string fileName,
            CancellationToken cancellationToken = default
            )
        {
            //string path = @"D:\Project";
            string path = configuration.GetSection("Mscrm").GetChildren().Where(x => x.Path == "Mscrm:Path").FirstOrDefault().Value;

            if (!Directory.Exists(path))

            {
                Directory.CreateDirectory(path);
            }

            string subFolderpath = Path.Combine(path, projectid.ToString());
            if (!Directory.Exists(subFolderpath))

            {
                Directory.CreateDirectory(subFolderpath);
            }
            var extension = Path.GetExtension(fileName);
            BinaryWriter Writer = new BinaryWriter(File.OpenWrite(subFolderpath + "\\" + projectid + extension));

            // Writer raw data  
            Writer.Write(content);
            Writer.Flush();
            Writer.Close();

            return Result.Success();
        }

        public async Task<Maybe<byte[]>> GetAttachmentAsync(string url, CancellationToken cancellationToken = default)
        {
            //string Url = @"D:\Project" + url;
            var Url = configuration.GetSection("Mscrm").GetChildren().Where(x => x.Path == "Mscrm:Path").FirstOrDefault().Value + url;
            FileStream fs = new FileStream(Url, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);

            br.Close();
            fs.Close();
            return bytes;
        }

        public async Task<Maybe<byte[]>> GetRoleAttachmentAsync(string url, CancellationToken cancellationToken = default)
        {
            var extension = ".pdf";
            var Url = configuration.GetSection("Mscrm").GetChildren().Where(x => x.Path == "Mscrm:RolesUserManual").FirstOrDefault().Value + "/" + url + "/" + url + extension;
            FileStream fs = new FileStream(Url, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);

            br.Close();
            fs.Close();
            return bytes;
        }
    }
}
