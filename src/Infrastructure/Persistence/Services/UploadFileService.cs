using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.Infrastructure.Persistence.Models;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Services
{
    internal sealed class UploadFileService : IUploadFileService
    {
        private readonly IConfiguration configuration;
        private readonly IChatRepository chatRepository;

        public UploadFileService(IConfiguration configuration, IChatRepository chatRepository)
        {
            this.configuration = configuration;
            this.chatRepository = chatRepository;
        }

        public async Task<Maybe<byte[]>> Download(string url, Guid roomId, CancellationToken cancellationToken = default)
        {
            var path = configuration.GetSection("Chat").GetChildren().Where(x => x.Path == "Chat:Path").FirstOrDefault().Value + @"\" + roomId.ToString() + @"\" + url;

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);

            br.Close();
            fs.Close();
            return bytes;
        }

        public async Task<Result<string>> Upload(Guid fileId, Guid roomId, Guid messageId, byte[] content, string contentType, string fileName, CancellationToken cancellationToken = default)
        {
            //string path = @"D:\Chat";
            string path = configuration.GetSection("Chat").GetChildren().Where(x => x.Path == "Chat:Path").FirstOrDefault().Value;

            if (!Directory.Exists(path))

            {
                Directory.CreateDirectory(path);
            }

            string subFolderpath = Path.Combine(path, roomId.ToString());

            if (!Directory.Exists(subFolderpath))

            {
                Directory.CreateDirectory(subFolderpath);
            }

            fileName = Path.ChangeExtension(fileName, null);
            var filePath = subFolderpath + "\\" + fileId.ToString();
            BinaryWriter Writer = new BinaryWriter(File.OpenWrite(filePath));

            // Writer raw data  
            Writer.Write(content);
            Writer.Flush();
            Writer.Close();

            var fileDb = new ChatData.Models.File { Id = fileId, Path = fileId.ToString(), FileName = fileName, ContentType = contentType, MessageId = messageId };

            await chatRepository.AddFile(fileDb, cancellationToken);

            return Result.Success(fileId.ToString());
        }
    }
}
