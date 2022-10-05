namespace eShop.Areas.Admin.ViewModels.Product
{
	public class ListItemProductVM
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public int InStock { get; set; }

		public int? CategoryId { get; set; }
		public DateTime CreatedAt { get; set; }
		public string CategoryName { get; set; }
	}
}
