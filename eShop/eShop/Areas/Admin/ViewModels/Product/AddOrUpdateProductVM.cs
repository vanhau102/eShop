using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eShop.Areas.Admin.ViewModels.Product
{
	public class AddOrUpdateProductVM
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "{0} là bắt buộc")]
		[MinLength(3, ErrorMessage = "{0} không được ít hơn 3 ký tự")]
		[DisplayName("Tên sản phẩm")]
		public string Name { get; set; }

		[MaxLength(50, ErrorMessage = "{0} tối đa 50 ký tự")]
		[DisplayName("Mô tả sản phẩm")]
		public string? Description { get; set; }

		[Required(ErrorMessage = "{0} là bắt buộc")]
		[DisplayName("Giá sản phẩm")]
		public decimal Price { get; set; }

		[Required(ErrorMessage = "{0} là bắt buộc")]
		[DisplayName("Hàng tồn kho")]
		public int InStock { get; set; }

		public int? CategoryId { get; set; }
	}
}
