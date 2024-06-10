using ReactApp1.Server.Models;

namespace ReactApp1.Server.Helpers
{
    public static class FileHelper
    {
        public static async Task<string> SaveAvatarImageAsync(UploadFileRequest uploadFileRequest, string userAvatarImage)
        {
            var uploadsFolder = Path.Combine("wwwRoot", "Avatars");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string path = Path.Combine(uploadsFolder, userAvatarImage ?? "");
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            var uniqueFileName = uploadFileRequest.UserId + "_" + uploadFileRequest.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await uploadFileRequest.FormFile.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }
    }
}
