using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NichTourTravel.WebSite.Helpers
{
    public class ImageHelper
    {
        public static (bool check, string url) SaveImage(IFormFile file, IWebHostEnvironment _hostingEnvironment)
        {
            if (file != null && file.Length > 0)
            {
                var uploadPath = Path.Combine("images/post/", Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName));
                using var stream = File.Create(_hostingEnvironment.WebRootPath + '/' + uploadPath);
                try
                {
                    file.CopyToAsync(stream).GetAwaiter().GetResult();
                }
                catch (Exception ex) { return (false, ex.Message); }

                return (true, "/" + uploadPath);
            }

            return (false, null);
        }
    }
}
