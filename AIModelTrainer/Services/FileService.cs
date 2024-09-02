using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AIModelTrainer.Services
{
    public class FileService
    {
        public async Task<string> DownloadImageFromUrlAsync(string url)
        {
            using var client = new HttpClient();

            try
            {
                using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);                

                if (!response.IsSuccessStatusCode)
                {
                    Console.Write($"\nUnable to retrieve image from URL. Status Code: {response.StatusCode}");                      
                    return null;
                }

                var imageBytes = await response.Content.ReadAsByteArrayAsync();
                var imagePath = Path.GetTempFileName();
                await File.WriteAllBytesAsync(imagePath, imageBytes);
                return imagePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError downloading image: {ex.Message}");
                return null;
            }
        }

        public string GetValidatedFilePath(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                Console.Write("File path cannot be empty. Press enter to return to the menu.");
                Console.ReadLine();
                return null;
            }

            return RemoveDoubleQuotes(input);
        }

        private string RemoveDoubleQuotes(string imagePath)
        {
            return imagePath.Trim('"');
        }
    }
}
