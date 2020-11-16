using System.IO;
using DrillHub.Model;

namespace DrillHub.ConsoleRunner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var categoryNamePrefix = new string(' ', 4);
            //var subCategoryNamePrefix = new string(' ', 8);

            var folderPath = @"D:\Projects\DrillHub\Docs";
            var fileName = "Копия Prays-list_25_09_2020.xlsx";
            var pathFile = Path.Combine(folderPath, fileName);

            var service = DependencyInjector.Instance.Resolve<ExcelService>();

            using var stream = File.OpenRead(pathFile);
            service.UpdateDataBaseFromStream(stream, categoryNamePrefix);
        }
    }
}
