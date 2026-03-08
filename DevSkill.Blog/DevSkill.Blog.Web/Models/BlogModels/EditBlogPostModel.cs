namespace DevSkill.Blog.Web.Models.BlogModels
{
    public class EditBlogPostModel
    {
        public Guid Id { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public List<string> Categories { get; set; } = new List<string>();
        public List<string> Tags { get; set; } = new List<string>();

        public async Task<string?> UploadThumbnailAsync(IWebHostEnvironment environment)
        {
            if (Thumbnail is not null)
            {
                var uploadPath = Path.Combine(environment.WebRootPath, "thumbnails");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var thumbnailName = $"{Guid.NewGuid().ToString().Substring(0, 10)}_{Thumbnail.FileName}";
                var thumbnailPath = Path.Combine(uploadPath, thumbnailName);

                using (var stream = new FileStream(thumbnailPath, FileMode.Create))
                {
                    await Thumbnail.CopyToAsync(stream);
                }

                return thumbnailName;
            }

            return null;
        }
    }
}
