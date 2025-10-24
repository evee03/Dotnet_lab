using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace Laboratorium_4.Pages
{
    public class UploadModel : PageModel
    {
        private readonly string imagesDir;

        [BindProperty]
        public IFormFile Upload { get; set; }

        public UploadModel(IWebHostEnvironment environment)
        {
            imagesDir = Path.Combine(environment.WebRootPath, "images");
        }

        public IActionResult OnPost()
        {
            const long MaxFileSize = 1 * 1024 * 1024;
            string extension = ".jpg";


            if (Upload != null)
            {
                if (Upload.Length > MaxFileSize)
                {
                    ModelState.AddModelError("Upload", "Plik jest za du¿y (max. 1 MB)");
                    return Page();
                }

                switch (Upload.ContentType)
                {
                    case "image/png":
                        extension = ".png";
                        break;
                    case "image/gif":
                        extension = ".gif";
                        break;
                }

                var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;
                var filePath = Path.Combine(imagesDir, fileName);

                using (var fs = System.IO.File.OpenWrite(filePath))
                {
                    Upload.CopyTo(fs);
                }

                using var image = new MagickImage(filePath);
                using var watermark = new MagickImage("watermark.png");

                watermark.Evaluate(Channels.Alpha, EvaluateOperator.Divide, 4);

                image.Composite(watermark, Gravity.Southeast, CompositeOperator.Over);

                image.Write(filePath);

            }

            return RedirectToPage("Index");
        }

        public void OnGet()
        {
        }
    }
}
