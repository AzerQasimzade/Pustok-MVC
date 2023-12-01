
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
                if(file.Length < 1024 * size)
            {
                return true;
            }
            return false;
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


    }
}
