using Dapper;
using Entity_Layer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace BookStore.Controllers
{
	public class ReportsController : Controller
	{
		private readonly string connectionString;
		public ReportsController(IConfiguration config)
		{
			connectionString = config.GetConnectionString("DefaultConnection");
		}

		public IDbConnection getConnection()
		{
			IDbConnection db = new SqlConnection(connectionString);
			return db;
		}
		// GET: ReportsController
		public ActionResult Index()
		{
			List<BookCategories> b_categories;
			using(IDbConnection db = getConnection())
			{
				String _query = "SELECT categoryID,COUNT(*) AS sumCategory FROM [BookCategories] GROUP BY categoryID";
				b_categories = db.Query<BookCategories>(_query).ToList();
			}
			var categories = b_categories.Select(c => c.categoryID);
			var categoriesSum = b_categories.Select(c => c.sumCategory);
			ViewBag.CATEGORIES = categories;
			ViewBag.SUM = categoriesSum;
			return View();
		}

		// GET: ReportsController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: ReportsController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: ReportsController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: ReportsController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: ReportsController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: ReportsController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: ReportsController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
