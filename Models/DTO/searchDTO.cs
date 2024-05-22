namespace prjAjaxDemo.Models.DTO
{
    public class searchDTO
    {
        public int categoryId { get; set; } = 0;
        public string? keyword { get; set; }
        public string? sortBy { get; set; }
        public string? sortType { get; set; } = "asc";
        public int? page { get; set; } = 1;
        public int? pageSize { get; set; } = 9;
    }
}
