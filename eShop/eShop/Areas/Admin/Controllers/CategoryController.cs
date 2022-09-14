using AutoMapper;
using AutoMapper.QueryableExtensions;
using eShop.Areas.Admin.ViewModels.Category;
using eShop.Database;
using eShop.Database.Entities;
using eShop.WebConfigs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.Areas.Admin.Controllers
{
	public class CategoryController : BaseController
	{
		private readonly IMapper _mapper;
		public CategoryController(AppDbContext db, IMapper mapper) : base(db)
		{
			_mapper = mapper;
		}

		public IActionResult Index()
		{
			return View();
		}

		public List<ListItemCategoryVM> GetAll()
		{
			var query = _db.ProductCategories
						.ProjectTo<ListItemCategoryVM>(AutoMapperProfile.CategoryConfig)
						.OrderByDescending(c => c.Id);
			var data = query.ToList();
			return data;
		}

		public IActionResult Create() => View();

		[HttpPost]
		public IActionResult Create([FromBody] AddOrUpdateCategoryVM categoryVM)
		{
			// xác thực dữ liệu
			if (ModelState.IsValid == false)
			{
				return Ok(new
				{
					success = false,
					mesg = "Không thể xoá vì danh mục đã được sử dụng"
				});
			}
			// Lưu vào db
			var category = new ProductCategory();
			//copy data từ category vào category
			_mapper.Map(categoryVM,category);
			category.CreatedAt = DateTime.Now;
			category.UpdatedAt = DateTime.Now;
			_db.ProductCategories.Add(category);
			_db.SaveChanges();
			return Ok(new
			{
				success = true
			});
		}
		public IActionResult Update (int id)
		{
			var category = _db.ProductCategories
				.ProjectTo<AddOrUpdateCategoryVM>(AutoMapperProfile.CategoryConfig)
				.FirstOrDefault(c => c.Id == id);
			if (category == null) return RedirectToAction(nameof(Index));
			return View(category);
		}

		public IActionResult GetForUpdate(int id)
		{
			var category = _db.ProductCategories
				.ProjectTo<AddOrUpdateCategoryVM>(AutoMapperProfile.CategoryConfig)
				.FirstOrDefault(c => c.Id == id);

			return Ok(category);
		}


		[HttpPost]
		public IActionResult Update(int id,[FromBody] AddOrUpdateCategoryVM categoryVM)
		{
			if(ModelState.IsValid== false)
			{
				return Ok(new
				{
					success = false,
					mesg = "Cập nhật danh mục thất bại !"
				});
			}
			var category = _db.ProductCategories.Find(id);

			if (category != null)
			{
				_mapper.Map(categoryVM,category);
				category.UpdatedAt = DateTime.Now;
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
			if (_db.Products.Any(p=>p.CategoryId== id))
			{
				//return RedirectToAction(nameof(Index));
				return Ok(new
				{
					success = false,
					mesg = "Không thể xoá vì danh mục đã được sử dụng"
				});
			}
				var category = _db.ProductCategories.Find(id);
				if (category != null)
				{
					_db.Remove(category);
					_db.SaveChanges();
				}

			return Ok(new
			{
				success = true
			});

		}
	}
}
