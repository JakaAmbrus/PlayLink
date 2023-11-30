namespace Application.Models
{
    public class PhotoUploadResult
    {
        public string PublicId { get; set; }
        public string Url { get; set; }

    }

    public class PhotoDeletionResult
    {
        public bool Succeeded { get; set; }
    }
}
