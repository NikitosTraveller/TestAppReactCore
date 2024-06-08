namespace ReactApp1.Server.Models
{
    public class UploadFileRequest
    {
        public string FileName {  get; set; }

        public IFormFile FormFile { get; set; }

        public string UserId { get; set; }
    }
}
