using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Shared.Helper
{
    public class PagedRequestBase
    {
       [Required]
       public int PageIndex { get; set; }
     
       [Required]
       public int PageSize { get; set; }
      
       public string? SearchTerm { get; set; }
       public string? OrderBy { get; set; }
    }
}
