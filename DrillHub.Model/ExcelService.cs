using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DrillHub.ConsoleRunner;
using DrillHub.Infrastructure;
using DrillHub.Model.Categories;
using DrillHub.Model.Products;
using DrillHub.Model.SubCategories;
using NPOI.SS.UserModel;

namespace DrillHub.Model
{
    public class ExcelService
    {
        private readonly IRepository<Category, int> _categoryRepository;
        private readonly IRepository<SubCategory, int> _subCategoryRepository;
        private readonly IRepository<Product, int> _productRepository;

        public ExcelService(
            IRepository<Category, int> categoryRepository,
            IRepository<SubCategory, int> subCategoryRepository,
            IRepository<Product, int> productRepository)
        {
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _productRepository = productRepository;
        }

        public async Task InsertCategoriesWithSubCategoriesWithProductAsync(List<Category> categories)
        {
            _categoryRepository.InsertRange(categories);
            await _categoryRepository.SaveChangesAsync();
        }

        public void InsertOrUpdateCategoriesWithSubCategoriesWithProduct(List<Category> categories)
        {
            //var dbCategories = _categoryRepository
            //    .Query(c=>c.SubCategories).ToList();
            
            //foreach (var category in categories)
            //{
            //    var dbCategory = dbCategories.FirstOrDefault(item => item.Name == category.Name);
            //    if (dbCategory == null)
            //    {
            //        _categoryRepository.Insert(category);
            //        _categoryRepository.SaveChanges();
            //    }

            //    foreach (var subCategory in category.SubCategories)
            //    {
            //        var dbSubCategory = dbCategories.SelectMany(item => item.SubCategories)
            //            .FirstOrDefault(item => item.Name == subCategory.Name);

            //        if (dbSubCategory == null)
            //        {
            //            subCategory.CategoryId = category.Id;
            //            _subCategoryRepository.Insert(subCategory);
            //            _subCategoryRepository.SaveChanges();
            //        }

            //        foreach (var product in subCategory.Products)
            //        {
            //            var dbProduct = dbCategories
            //                .SelectMany(item => item.SubCategories)
            //                .SelectMany(item => item.Products)
            //                .FirstOrDefault(item => item.VendorCode == product.VendorCode || item.OriginalName == product.OriginalName);

            //            if (dbProduct == null)
            //            {
            //                _productRepository.Insert(product);
            //                _subCategoryRepository.SaveChanges();
            //            }
            //            else
            //            {
            //                dbProduct.SubCategoryId = subCategory.Id;
            //                dbProduct.OriginalName = product.OriginalName;
            //                dbProduct.DisplayName = product.DisplayName;
            //                dbProduct.Price = product.Price;
            //                dbProduct.UnitType = product.UnitType;
            //                dbProduct.QuantityInStock = product.QuantityInStock;
            //            }
            //        }
            //    }
            //}
            //_categoryRepository.InsertRange(categories);
            //_categoryRepository.SaveChanges();
        }

        public async Task UpdateDataBaseFromStream(FileStream stream, string categoryNamePrefix)
        {
            var categories = ParseExcel(stream, categoryNamePrefix);
            await InsertCategoriesWithSubCategoriesWithProductAsync(categories);
        }

        public List<Category> ParseExcel(FileStream stream, string categoryNamePrefix)
        {
            var workbook = WorkbookFactory.Create(stream);

            var sheet = workbook.GetSheetAt(0);

            var ranges = GetRanges(sheet, 1, categoryNamePrefix);

            var result =
                GetCategoriesWithSubCategoriesWithProduct(sheet, ranges.First(), categoryNamePrefix);

            var categoriesCount = result.Count();
            var subCategoriesCount = result.SelectMany(item => item.SubCategories).Count(item => !item.IsCategory);
            var product = result.SelectMany(item => item.SubCategories).SelectMany(item => item.Products).Count();

            return result;
        }

        public List<Category> GetCategoriesWithSubCategoriesWithProduct(
           ISheet sheet,
           Range range,
           string categoryNamePrefix)
        {
            var categories = new List<Category>();
            var currentCategory = new Category();
            var currentSubCategory = new SubCategory();

            for (var index = range.StartIndex; index <= range.EndIndex; index++)
            {
                //Console.WriteLine($"Process {i}/{range.EndIndex}");
                var vendorCodeCell = sheet.GetRow(index).GetCell(1);

                var isProduct = vendorCodeCell.CellType == CellType.Numeric;
                var vendorCode = isProduct ? vendorCodeCell.NumericCellValue.ToString(CultureInfo.InvariantCulture) : "";
                var name = sheet.GetRow(index).GetCell(2).StringCellValue;

                var countSpaceFromStartInName = name.Split(' ').Count(string.IsNullOrWhiteSpace);

                if (string.IsNullOrWhiteSpace(vendorCode))
                {
                    if (countSpaceFromStartInName == categoryNamePrefix.Length)
                    {
                        var category = new Category
                        {
                            Name = name.Trim(),
                            SubCategories = new List<SubCategory>()
                        };

                        currentCategory = category;
                        categories.Add(category);

                        currentSubCategory = null;

                        continue;
                    }

                    if (countSpaceFromStartInName > categoryNamePrefix.Length)
                    {
                        var subCategory = new SubCategory
                        {
                            Name = name.Trim(),
                            Products = new List<Product>(),
                            IsCategory = false
                        };

                        currentSubCategory = subCategory;

                        currentCategory.SubCategories.Add(subCategory);
                    }
                }

                else
                {
                    var quantityInStock = (int)sheet.GetRow(index).GetCell(3).NumericCellValue;
                    var price = (decimal)sheet.GetRow(index).GetCell(4).NumericCellValue;

                    var test = sheet.GetRow(index).GetCell(5);
                    var unitType = sheet.GetRow(index).GetCell(5).StringCellValue.Contains("шт") ? UnitType.Single : UnitType.Collection;

                    var trimName = name.Trim();

                    var product = new Product
                    {
                        DisplayName = trimName,
                        OriginalName = trimName,
                        VendorCode = vendorCode.Trim(),
                        QuantityInStock = quantityInStock,
                        Price = price,
                        UnitType = unitType
                    };

                    if (currentSubCategory == null)
                    {
                        currentSubCategory = new SubCategory
                        {
                            Name = currentCategory.Name.Trim(),
                            IsCategory = true,
                            Products = new List<Product>()
                        };
                        currentCategory.SubCategories.Add(currentSubCategory);
                    }

                    currentSubCategory.Products.Add(product);
                }
            }
            return categories;
        }

        public List<Range> GetRanges(ISheet sheet, int countThread, string categoryNamePrefix)
        {
            var length = sheet.LastRowNum;
            var cutoff = length / countThread;

            var startIndexes = new List<int>();

            for (var threadNumber = 0; threadNumber < countThread; threadNumber++)
            {
                var currentCutoffValue = threadNumber * cutoff;
                var nextCutoffValue = (threadNumber + 1) * cutoff;

                for (var index = currentCutoffValue; index < nextCutoffValue; index++)
                {
                    var vendorCodeCell = sheet.GetRow(index).GetCell(1);
                    var name = sheet.GetRow(index).GetCell(2)?.StringCellValue;
                    var countSpaceFromStartInName = name?.Split(' ').Count(string.IsNullOrWhiteSpace);

                    var isCategory = vendorCodeCell?.CellType == CellType.Blank
                                     && string.IsNullOrWhiteSpace(vendorCodeCell?.StringCellValue)
                                     && countSpaceFromStartInName == categoryNamePrefix.Length;

                    if (isCategory)
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
    }
}
