using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace TrainingScheduler
{
    public class DropboxManager
    {
        private string accessToken = "HmZTH3kwY3AAAAAAAAAAEFykE4wdvaDmVW7jceRUYRvVykuz9nNvvVDezY2ylnaK";
        private DropboxClient client;

        public DropboxManager()
        {
            client = new DropboxClient(accessToken);
        }

        public async Task<bool> CheckFileExists(string name)
        {
            var list = await client.Files.ListFolderAsync("");
            foreach (var item in list.Entries)
            {
                if (item.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task Download(string name)
        {
            Debug.WriteLine("Download file {0} ...", name, null);

            using (var response = await client.Files.DownloadAsync("/" + name))
            {
                using (var fileStream = File.Create(name))
                {
                    (await response.GetContentAsStreamAsync()).CopyTo(fileStream);
                }
            }
        }

        public async Task Upload(string fileName, string fileContent)
        {
            Debug.WriteLine("Upload file...");

            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent)))
            {
                var response = await client.Files.UploadAsync("/" + fileName, WriteMode.Overwrite.Instance, body: stream);

                Debug.WriteLine("Uploaded Id {0} Rev {1}", response.Id, response.Rev);
            }
        }

        public async Task Upload(string fileName)
        {
            using (MemoryStream mem = new MemoryStream(File.ReadAllBytes(fileName)))
            {
                var updated = await client.Files.UploadAsync(
                    "/" + fileName,
                    WriteMode.Overwrite.Instance,
                    body: mem);
                Debug.WriteLine("Saved {0} rev {1}", fileName, updated.Rev);
            }
        }

        public async Task<bool> Delete(string filename)
        {
            try
            {
                Debug.WriteLine("delete file /{0}", filename, null);
                DeleteResult result = await client.Files.DeleteV2Async("/" + filename);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error {0}", ex.ToString(), null);
                return false;
            }
        }
    }
}
