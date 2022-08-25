using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Injection
{
    public static class FileExtension
    {
        public static async Task<string> CreateFile(this IFormFile file, string userDirectory)
        {
            try
            {
                if (file.Length > 0)
                {
                    var img = Guid.NewGuid();
                    var directoryPath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\" + userDirectory;
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    using var stream = new FileStream($"{directoryPath}/{img}{Path.GetExtension(file.FileName)}", FileMode.Create);

                    await file.CopyToAsync(stream);

                    return $"{img}{Path.GetExtension(file.FileName)}";
                }
                return string.Empty;

            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static void DeleteFile(string filePath)
        {
            try
            {
                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath);
                }
                else
                {
                    throw new ArgumentException("Directory not exists", nameof(filePath));
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message, nameof(filePath));
            }

        }
    }
}
