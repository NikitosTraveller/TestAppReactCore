using System.ComponentModel.DataAnnotations;

namespace ReactApp1.Server.Models
{
    public class UploadFileRequest
    {
        [FileExtensions(Extensions = "jpg,jpeg,svg,png")]
        public string FileName {  get; set; }

        [Required]
        public IFormFile FormFile { get; set; }

        public string UserId { get; set; }
    }
}
