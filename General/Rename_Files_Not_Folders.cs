// Rename all nested files in all sub folders but NOT the folders themselves
namespace ConsoleApp1
{
    internal class Program
    {
        static void Main()
        {
            string folderPath = @"C:\Users\Crocodile\Desktop\";

            try
            {
                var directoryInfo = new DirectoryInfo(folderPath);
                var directories = directoryInfo.GetDirectories();
                foreach (var directory in directories )
                {
                    foreach (FileInfo fileInfo in directory.GetFiles())
                    {
                        var newFileName = GenerateRandomGuid() + fileInfo.Extension;
                        var newPath = Path.Combine(Path.Combine(folderPath, directory.Name), newFileName);
                        fileInfo.MoveTo(newPath);
                    }
                }                

                Console.WriteLine("All files renamed to random GUIDs.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static string GenerateRandomGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
