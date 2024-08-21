using Microsoft.AspNetCore.Http;

namespace Social.Application.Features.Posts.Common
{
    public class PostContentDto
    {
        public string Description { get; set; }
        public IFormFile PhotoFile { get; set; }
    }
}
