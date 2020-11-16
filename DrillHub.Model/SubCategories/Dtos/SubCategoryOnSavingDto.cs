namespace DrillHub.Model.SubCategories.Dtos
{
    public class SubCategoryOnSavingDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool IsCategory { get; set; }
    }
}
