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
		
		public ActionResult Index()
		{
			List<Categories> categories;
			using (IDbConnection db = getConnection())
			{
				string _query = "Select * From Categories";
				categories = db.Query<Categories>(_query).ToList();
			}
			return View(categories);
		}

		public List<Categories> GetAllCategories()
        {
			List<Categories> categories;
			using (IDbConnection db = getConnection())
			{
				string _query = "Select * From Categories";
				categories = db.Query<Categories>(_query).ToList();
			}
			return categories;
		}

		public ActionResult ShowBooks(int id)
		{
			List<Books> books;
			using (IDbConnection connection = getConnection())
			{
				
				string _query = @"Select * FROM BookCategories a 
				LEFT JOIN (Select [bookName],[bookID],[bookPages],[bookPrice] From Books) b 
				ON a.bookID=b.bookID WHERE categoryID=@id";
				books = connection.Query<Books>(_query,new {id=id}).ToList();
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
					
					string _query = @"Select * FROM [BookCategories] a 
									LEFT JOIN (Select * From [Books] c LEFT JOIN [Authors] d ON c.[bookAuthorID] = d.[authorID]) b 
									ON a.[bookID]=b.[bookID] WHERE [categoryID]=@id";
					books = connection.Query<Books>(_query,new {id=id}).ToList();
				}
            }

			return books;
		}


		public IDbConnection getConnection()
		{
			IDbConnection temp = new SqlConnection(connectionString);
			return temp;
		}

		
		
		[HttpPost]
		public ActionResult Create(Categories c)
		{
			string _query = "INSERT INTO Categories (categoryName) VALUES (@categoryName)";
			try
			{
				using (IDbConnection connection = getConnection())
				{
					
					connection.Execute(_query,c);
				}
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		
		public ActionResult Edit(int id)
		{
			Categories c;
			String _query = "SELECT * FROM Categories WHERE categoryID=@id";
			using(IDbConnection connection = getConnection())
			{
				c = connection.Query<Categories>(_query,new {id=id}).SingleOrDefault();
			}
			return View(c);
		}

		
		[HttpPost]
		public ActionResult Edit(Categories c)
		{
			string _query = "UPDATE Categories SET categoryName=@categoryName WHERE categoryID=@categoryID";
			try
			{
				using (IDbConnection connection = getConnection())
				{
					connection.Execute(_query,c);
				}
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		

		
		[HttpPost]
		public ActionResult Delete(int id)
		{
			string _categoryQuery = "DELETE FROM BookCategories WHERE categoryID=@id";
			string _query = "DELETE FROM Categories WHERE categoryID=@id";
			using (IDbConnection connection = getConnection())
			{
				connection.Execute(_categoryQuery, new {id= id });
				connection.Execute(_query, new { id = id });
			}
			return RedirectToAction(nameof(Index));
		}
	}
}
