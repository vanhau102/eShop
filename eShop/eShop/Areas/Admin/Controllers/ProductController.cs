using AutoMapper;
using AutoMapper.QueryableExtensions;
using eShop.Areas.Admin.ViewModels.Product;
using eShop.Database;
using eShop.Database.Entities;
using eShop.WebConfigs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eShop.Areas.Admin.Controllers
{
	public class ProductController : BaseController
	{
		private readonly IMapper _mapper;
		public ProductController(AppDbContext db, IMapper mapper) : base(db)
		{
			_mapper = mapper;
		}
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var method = context.HttpContext.Request.Method;
			if (method == HttpMethod.Post.Method)
			{
				if (!ModelState.IsValid)
				{
					var errorModel = new SerializableError(ModelState);
					context.Result = new BadRequestObjectResult(errorModel);
				}
			}
		}
		public IActionResult Index()
		{
			return View();
		}
		public List<ListItemProductVM> GetAll()
		{
			var query = _db.Products
						.ProjectTo<ListItemProductVM>(AutoMapperProfile.ProductConfig)
						.OrderByDescending(c => c.Id);
			var data = query.ToList();
			return data;
		}
		[HttpPost]
		public IActionResult Create([FromBody] AddOrUpdateProductVM productVM )
		{
			if (ModelState.IsValid == false)
			{
				return Ok(new
				{
					success = false,
					mesg = "Không thể thêm sản phẩm"
				});
			}
			// Lưu vào db
			var product = new Product();
			//copy data từ productVM vào product
			_mapper.Map(productVM, product);
			product.CreatedAt = DateTime.Now;
			product.UpdatedAt = DateTime.Now;
			_db.Products.Add(product);
			_db.SaveChanges();
			return Ok(new
			{
				success = true
			});
		}
		public IActionResult GetForUpdate(int id)
		{
			var product = _db.Products
				.ProjectTo<AddOrUpdateProductVM>(AutoMapperProfile.ProductConfig)
				.FirstOrDefault(c => c.Id == id);

			return Ok(product);
		}


		[HttpPost]
		public IActionResult Update(int id, [FromBody] AddOrUpdateProductVM productVM)
		{
			if (ModelState.IsValid == false)
			{
				return Ok(new
				{
					success = false,
					mesg = "Cập nhật sản phẩm thất bại !"
				});
			}
			var product = _db.Products.Find(id);

			if (product != null)
			{
				_mapper.Map(productVM, product);
				product.UpdatedAt = DateTime.Now;
				_db.SaveChanges();
			}
			return Ok(new
			{
				success = true
			});

		}
		public IActionResult Delete(int id)
		{
			//Không cho xoá nếu danh mục đã có sản phẩm
			if (ModelState.IsValid == false)
			{
				//return RedirectToAction(nameof(Index));
				return Ok(new
				{
					success = false,
					mesg = "Không thể xoá sản phẩm "
				});
			}
			var product = _db.Products.Find(id);
			if (product != null)
			{
				_db.Remove(product);
				_db.SaveChanges();
			}

			return Ok(new
			{
				success = true
			});

		}
	}
}
