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
			var query = _db.ProductCategories
						.ProjectTo<ListItemCategoryVM>(AutoMapperProfile.CategoryConfig)
						.OrderByDescending(c => c.Id);
			ViewBag.Sql = query.ToQueryString();
			var data = query.ToList();
			return View("Index", data);
		}

		public IActionResult Create() => View();

		[HttpPost]
		public IActionResult Create(AddOrUpdateCategoryVM categoryVM)
		{
			// xác thực dữ liệu
			if (ModelState.IsValid == false)
			{
				return View(categoryVM);
			}
			// Lưu vào db
			var category = new ProductCategory();
			//copy data từ category vào category
			_mapper.Map(categoryVM,category);
			category.CreatedAt = DateTime.Now;
			category.UpdatedAt = DateTime.Now;
			_db.ProductCategories.Add(category);
			_db.SaveChanges();
			return RedirectToAction(nameof(Index));
		}
		public IActionResult Update (int id)
		{
			var category = _db.ProductCategories
				.ProjectTo<AddOrUpdateCategoryVM>(AutoMapperProfile.CategoryConfig)
				.FirstOrDefault(c => c.Id == id);

			return View(category);
		}
		[HttpPost]
		public IActionResult Update(int id, AddOrUpdateCategoryVM categoryVM)
		{
			if(ModelState.IsValid== false)
			{
				return View(categoryVM);
			}
			var category = _db.ProductCategories.Find(id);

			if (category != null)
			{
				_mapper.Map(categoryVM,category);
				category.UpdatedAt = DateTime.Now;
				_db.SaveChanges();
			}
			return RedirectToAction(nameof(Index));

		}

		public IActionResult Delete(int id)
		{
			//Không chol xoá nếu danh mục đã có sản phẩm
			if (_db.Products.Any(p=>p.CategoryId== id))
			{
				TempData["Err"] = "Danh mục đã được sử dụng, không thể xoá ";
				return RedirectToAction(nameof(Index));
			}
			var category = _db.ProductCategories.Find(id);
			if(category != null)
			{
				_db.Remove(category);
				_db.SaveChanges();
			}
			TempData["SuccesMesg"] = $"Xoá danh mục {category.Name} thành công ";

			return RedirectToAction(nameof(Index));
		}
	}
}
