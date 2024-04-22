using System.ComponentModel.DataAnnotations;

namespace Store.Service.Services.BasketService.Dtos
{
    public class BasketItemDto
    {
        [Required]
        [Range(1,int.MaxValue)]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        [Range(0.1,double.MaxValue,ErrorMessage ="Price Must Be Greater Than Zero")]
        public decimal Price { get; set; }
        [Required]
        [Range(1,int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string CategoryName { get; set; }
    }
}
