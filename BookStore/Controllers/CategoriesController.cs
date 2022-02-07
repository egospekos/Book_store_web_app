using Dapper;
using Entity_Layer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace BookStore.Controllers
{
	public class CategoriesController : Controller
	{
		private readonly string connectionString;
		public CategoriesController(IConfiguration config)
		{
			connectionString = config.GetConnectionString("DefaultConnection");
		}
		// GET: CategoryController
		public ActionResult Index()
		{
			List<Categories> categories;
			using (IDbConnection db = getConnection())
			{
				categories = db.Query<Categories>("Select * From Categories").ToList();
			}
			return View(categories);
		}

		public List<Categories> GetAllCategories()
        {
			List<Categories> categories;
			using (IDbConnection db = getConnection())
			{
				categories = db.Query<Categories>("Select * From Categories").ToList();
			}
			return categories;
		}

		public ActionResult ShowBooks(int id)
		{
			List<Books> books;
			using (IDbConnection connection = getConnection())
			{
				//LEFT JOIN Authors ON Books.bookAuthorID = Authors.authorID
				string _query = "Select * FROM BookCategories a LEFT JOIN (Select [bookName],[bookID],[bookPages],[bookPrice] From Books) b ON a.bookID=b.bookID WHERE categoryID=" + id;
				books = connection.Query<Books>(_query).ToList();
			}

			return View(books);
		}
		public ActionResult PartialShowBooks(int id)
		{
			List<Books> books = GetCategoryBooks(id);
			return PartialView(books);
		}
		public List<Books> GetCategoryBooks(int? id)
		{
			List<Books> books = new List<Books>();
            if (id != null)
            {

				using (IDbConnection connection = getConnection())
				{
					//LEFT JOIN Authors ON Books.bookAuthorID = Authors.authorID
					string _query = "Select * FROM BookCategories a LEFT JOIN (Select * From Books LEFT JOIN Authors ON Books.bookAuthorID = Authors.authorID) b ON a.bookID=b.bookID WHERE categoryID=" + id;
					books = connection.Query<Books>(_query).ToList();
				}
            }

			return books;
		}


		public IDbConnection getConnection()
		{
			IDbConnection temp = new SqlConnection(connectionString);
			return temp;
		}

		
		// POST: CategoryController/Create
		[HttpPost]
		public ActionResult Create(Categories c)
		{
			string _query = "INSERT INTO Categories (categoryName) VALUES (@categoryName)";
			try
			{
				using (IDbConnection connection = getConnection())
				{
					
					int rows = connection.Execute(_query,c);
				}
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: CategoryController/Edit/5
		public ActionResult Edit(int id)
		{
			Categories c;
			String _query = "SELECT * FROM Categories WHERE categoryID=" + id;
			using(IDbConnection connection = getConnection())
			{
				c=connection.Query<Categories>(_query).SingleOrDefault();
			}
			return View(c);
		}

		// POST: CategoryController/Edit/5
		[HttpPost]
		public ActionResult Edit(Categories c)
		{
			string _query = "UPDATE Categories SET categoryName=@categoryName WHERE categoryID=@categoryID";
			try
			{
				using (IDbConnection connection = getConnection())
				{
					int rows = connection.Execute(_query,c);
				}
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		

		// POST: CategoryController/Delete/5
		[HttpPost]
		public ActionResult Delete(int id)
		{
			string _categoryQuery = "DELETE FROM BookCategories WHERE categoryID=" + id;
			string _query = "DELETE FROM Categories WHERE categoryID=" + id;
			using (IDbConnection connection = getConnection())
			{
				int _rows = connection.Execute(_categoryQuery);
				int rows = connection.Execute(_query);
			}
			return RedirectToAction(nameof(Index));
		}
	}
}
