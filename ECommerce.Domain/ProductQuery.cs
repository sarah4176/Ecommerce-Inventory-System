namespace ECommerce.Domain
{
    public class ProductQuery
    {
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SearchTerm { get; set; }
        public int Page { get; set; } 
        public int Limit { get; set; } = 10;
    }
}
