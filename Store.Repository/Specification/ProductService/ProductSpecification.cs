namespace Store.Repository.Specification.ProductService
{
    public class ProductSpecification
    {
        private int _pageSize = 6;
        private const int MAXPAGESIZE = 50;

        public int? CategoryId { get; set; }
        public string? Sort {  get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize 
        {
            get => _pageSize;
            set => _pageSize = (value>MAXPAGESIZE)?MAXPAGESIZE:value; 
        }
    }
}
