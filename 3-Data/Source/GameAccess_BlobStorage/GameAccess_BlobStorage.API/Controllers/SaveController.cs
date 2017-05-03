using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace GameAccess_BlobStorage.API.Controllers
{
    [Route("api/[controller]")]
    public class SaveController : Controller
    {
        private IConfiguration _configuration { get; set; }

        public SaveController(IConfiguration config)
        {
            _configuration = config;
        }


        //[Authorize]
        [HttpGet]
        [Route("bloburl")]
        public async Task<string> GetBlobUrl()
        {
            var client = GetCloudBlobClient();
            var container = client.GetContainerReference(_configuration.GetValue<string>("BlobStorage:SavesContainer"));
            await container.CreateIfNotExistsAsync();

            var blobName = "save.sav"; // should be unique for a particular player
            var blob = container.GetBlockBlobReference(blobName);

            SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Create,
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
            };

            var sas = blob.GetSharedAccessSignature(policy);

            return blob.Uri + sas;
        }

        private CloudBlobClient GetCloudBlobClient()
        {
            var account = CloudStorageAccount.Parse(_configuration.GetValue<string>("BlobStorage:StorageAccountConnectionString"));
            var cloudBlobClient = account.CreateCloudBlobClient();

            return cloudBlobClient;
        }

    }
}
