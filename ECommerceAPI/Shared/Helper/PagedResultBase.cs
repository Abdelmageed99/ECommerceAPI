namespace ECommerceAPI.Shared.Helper
{
    public class PagedResultBase<T> where T : class
    {
        public List<T> Entities { get; set; }

        public int TotalRecords { get; set; }
        public int TotalPages { get; set; } = 0;
        public int CurrentPage { get; set; }
    }
}
