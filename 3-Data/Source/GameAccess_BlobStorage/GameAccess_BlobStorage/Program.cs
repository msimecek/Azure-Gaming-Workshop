using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GameAccess_BlobStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Let's save!");

            var saveGame = new SaveGame()
            {
                CharId = 1,
                CharName = "Martinus Maximus",
                Score = 1999
            };

            Console.WriteLine("Getting file SAS URL...");
            var sasUrl = GetSaveFileAddressAsync().Result;

            if (sasUrl == null)
            {
                Console.ReadKey();
                return;
            }

            Console.WriteLine(sasUrl);
            Console.WriteLine("Uploading save file...");

            CloudBlockBlob saveFile = new CloudBlockBlob(new Uri(sasUrl));
            saveFile.UploadTextAsync(saveGame.Serialize());

            Console.WriteLine("Done.");
            Console.ReadKey();

        }

        static async Task<string> GetSaveFileAddressAsync()
        {
            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    var resp = await hc.GetAsync($"{Config.ApiUrl}/save/bloburl");
                    if (resp.IsSuccessStatusCode)
                    {
                        return await resp.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine("Something went wrong while getting the key.");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
                
            }
        }

    }
}