
namespace PustokBookStore.Utilities.Extensions
{
    static public class FileExtensions
    {
            public static bool CheckFileType(this IFormFile file, string type)
            {
                return file.ContentType.Contains(type);
            }


            public static bool CheckFileLength(this IFormFile file, int size)
            {
                return file.Length > 1024 * size;
            }


            public static string CreateFile(this IFormFile file, string root, string folder)
            {
                string filename = Guid.NewGuid().ToString() + "_" + file.FileName;


                string path = Path.Combine(root, folder, filename);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return filename;
            }

            public static void DeleteFile(this string image, string root, string folder)
            {
                string path = Path.Combine(root, folder, image);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

        //    public static async Task<string> CreateFileAsync(this IFormFile file, string root, params string[] folders)
        //{
        //    string fileName = Guid.NewGuid().ToString() + file.FileName;
        //    string path = root;
        //    for (int i = 0; i < folders.Length; i++)
        //    {
        //        path = Path.Combine(path, folders[i]);
        //    }
        //    path = Path.Combine(path, fileName);
        //    using (FileStream fileStream = new FileStream(path, FileMode.Create))
        //    {
        //        await file.CopyToAsync(fileStream);
        //    }
        //    return fileName;
        //}
        //public static async void DeleteFile(this string filename, string root, params string[] folders)
        //{
        //    string path = root;
        //    for (int i = 0; i < folders.Length; i++)
        //    {
        //        path = Path.Combine(path, folders[i]);
        //    }
        //    path = Path.Combine(path, filename);
        //    if (File.Exists(path))
        //    {
        //        File.Delete(path);
        //    }
        //}


    }
}
