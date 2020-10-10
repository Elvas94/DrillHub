using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DrillHub.ConsoleRunner;
using DrillHub.Model.Categories;
using DrillHub.Model.Products;
using DrillHub.Model.Subcategories;
using Spire.Xls;

namespace DrillHub.Model
{
    public class ExcelService
    {
        public async Task UpdateDataBaseFromStream(
            FileStream stream,
            string categoryNamePrefix)
        {
            var categories = await ParseExcelAsync(stream, categoryNamePrefix);
        }

        public async Task<List<Category>> ParseExcelAsync(FileStream stream, string categoryNamePrefix)
        {
            var countThread = Environment.ProcessorCount / 2;

            var workbook = new Workbook();

            workbook.LoadFromStream(stream);

            if (!workbook.FileName.EndsWith(".xlsx"))
            {
                throw new ApplicationException(" Формат файла должен быть .xlsx");
            }

            var ranges = GetRanges(workbook, countThread, categoryNamePrefix);

            var tasks = ranges.Select(range =>
               Task.Run(() =>
                  GetCategoriesWithSubCategoriesWithProduct(
                   workbook,
                   range,
                   categoryNamePrefix)))
               .ToList();

            var result = (await Task.WhenAll(tasks)).SelectMany(item => item).ToList();

            //var categoriesCount = result.Count();
            //var subCategoriesCount = result.SelectMany(item => item.SubCategories).Count(item => !item.IsCategory);
            //var product = result.SelectMany(item => item.SubCategories).SelectMany(item => item.Products).Count();

            return result;
        }

        public async Task<List<Range>> GetRangesAsync(Workbook workbook, int countThread, string categoryNamePrefix)
        {
            var sheet = workbook.Worksheets[0];
            var length = sheet.Rows.Length;
            var cutoff = length / countThread;

            var tasks = Enumerable.Range(0, countThread).Select((item, index) =>
                  Task.Run(() =>
                     GetIndexStartByRange(
                        sheet,
                        new Range
                        {
                            StartIndex = index * cutoff,
                            EndIndex = (index + 1) * cutoff
                        },
                        categoryNamePrefix)))
               .ToList();

            var startIndexes = (await Task.WhenAll(tasks)).Where(item => item != -1).ToList();

            return startIndexes.Select((item, index) =>
               new Range
               {
                   StartIndex = item,
                   EndIndex = index == startIndexes.Count - 1 ? length : startIndexes[index + 1] - 1
               }).ToList();
        }

        public List<Range> GetRanges(Workbook workbook, int countThread, string categoryNamePrefix)
        {
            var sheet = workbook.Worksheets[0];
            var length = sheet.Rows.Length;
            var cutoff = length / countThread;

            var startIndexes = new List<int>();

            for (var threadNumber = 0; threadNumber < countThread; threadNumber++)
            {
                var currentCutoffValue = threadNumber * cutoff;
                var nextCutoffValue = (threadNumber + 1) * cutoff;

                for (var index = currentCutoffValue; index < nextCutoffValue; index++)
                {
                    var vendorCode = sheet.Rows[index].Columns[1].Value;
                    var name = sheet.Rows[index].Columns[2].Value;

                    var countSpaceFromStartInName = name.Split(' ').Count(string.IsNullOrWhiteSpace);

                    if (string.IsNullOrWhiteSpace(vendorCode) && countSpaceFromStartInName == categoryNamePrefix.Length)
                    {
                        startIndexes.Add(index);
                        break;
                    }
                }
            }

            return startIndexes.Select((item, index) =>
               new Range
               {
                   StartIndex = item,
                   EndIndex = index == startIndexes.Count - 1 ? length : startIndexes[index + 1] - 1
               }).ToList();
        }

        public int GetIndexStartByRange(Worksheet sheet, Range range, string categoryNamePrefix)
        {
            for (var index = range.StartIndex; index < range.EndIndex; index++)
            {
                var vendorCode = sheet.Rows[index].Columns[1].Value;
                var name = sheet.Rows[index].Columns[2].Value;

                var countSpaceFromStartInName = name.Split(' ').Count(string.IsNullOrWhiteSpace);

                if (string.IsNullOrWhiteSpace(vendorCode) && countSpaceFromStartInName == categoryNamePrefix.Length)
                {
                    return index;
                }
            }

            return -1;
        }

        public List<Category> GetCategoriesWithSubCategoriesWithProduct(
           Workbook workbook,
           Range range,
           string categoryNamePrefix)
        {
            var sheet = workbook.Worksheets[0];

            var categories = new List<Category>();
            var currentCategory = new Category();
            var currentSubCategory = new Subcategory();

            for (var i = range.StartIndex; i < range.EndIndex; i++)
            {
                //Console.WriteLine($"Process {i}/{range.EndIndex}");

                var vendorCode = sheet.Rows[i].Columns[1].Value;
                var name = sheet.Rows[i].Columns[2].Value;

                var countSpaceFromStartInName = name.Split(' ').Count(string.IsNullOrWhiteSpace);

                if (string.IsNullOrWhiteSpace(vendorCode))
                {
                    if (countSpaceFromStartInName == categoryNamePrefix.Length)
                    {
                        var category = new Category
                        {
                            Name = name.Trim(),
                            Subcategories = new List<Subcategory>()
                        };

                        currentCategory = category;
                        categories.Add(category);

                        currentSubCategory = null;

                        continue;
                    }

                    if (countSpaceFromStartInName > categoryNamePrefix.Length)
                    {
                        var subCategory = new Subcategory
                        {
                            Name = name.Trim(),
                            Products = new List<Product>(),
                            IsCategory = false
                        };

                        currentSubCategory = subCategory;

                        currentCategory.Subcategories.Add(subCategory);
                    }
                }

                else
                {
                    var quantityInStock = Int32.Parse(sheet.Rows[i].Columns[3].Value);
                    var price = Decimal.Parse(sheet.Rows[i].Columns[4].Value.Replace(".руб", ""));
                    var unitType = sheet.Rows[i].Columns[4].Value.Contains("шт") ? UnitType.Single : UnitType.Collection;

                    var product = new Product
                    {
                        Name = name.Trim(),
                        VendorCode = vendorCode.Trim(),
                        QuantityInStock = quantityInStock,
                        Price = price,
                        UnitType = unitType
                    };

                    if (currentSubCategory == null)
                    {
                        currentSubCategory = new Subcategory
                        {
                            Name = currentCategory.Name.Trim(),
                            IsCategory = true,
                            Products = new List<Product>()
                        };
                        currentCategory.Subcategories.Add(currentSubCategory);
                    }

                    currentSubCategory.Products.Add(product);
                }
            }
            return categories;
        }
    }
}
