using Dapper;
using Entity_Layer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace BookStore.Controllers
{
	public class AuthorsController : Controller
	{
		private readonly String connectionString;
		public AuthorsController(IConfiguration config)
		{
			connectionString = config.GetConnectionString("DefaultConnection");
		}
		// GET: AuthorsController
		public ActionResult Index()
		{
			List<Authors> Authors;
			using(IDbConnection db = getConnection())
			{
				string _query = "Select * From Authors a LEFT JOIN " +
					"(Select bookAuthorID as authorID,COUNT(*) as countBook FROM Books GROUP BY bookAuthorID) b " +
					"ON a.authorID = b.authorID ";
				Authors = db.Query<Authors>(_query).ToList();
			}
			return View(Authors);
		}

		public ActionResult ShowBooks(int id)
		{
			List<Books> Books = GetBooks(id);
			return View(Books);
		}
		public ActionResult PartialShowbooks(int id)
		{
			List<Books> Books = GetBooks(id);
			return PartialView();
		}
		
		public List<Books> GetBooks(int id)
		{
			List<Books> books;
			using (IDbConnection db = getConnection())
			{
				string _query = "Select * From Books WHERE bookAuthorID=" + id;
				books = db.Query<Books>(_query).ToList();
			}
			return books;
		}
		

		// GET: AuthorsController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: AuthorsController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: AuthorsController/Create
		[HttpPost]
		public ActionResult Create(Authors p)
		{
			try
			{
				using (IDbConnection connection = getConnection())
				{
					int rows = connection.Execute("INSERT INTO Authors VALUES(@authorName,@authorBday)",p);
				}
					return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		public ActionResult PartialCreate()
		{
			return PartialView("PartialCreate");
		}

		// GET: AuthorsController/Edit/5
		public ActionResult Edit(int id)
		{
			Authors p;
			string _query = "Select * From Authors WHERE authorID=" + id;
			using(IDbConnection connection = getConnection())
			{
				p = connection.Query<Authors>(_query).SingleOrDefault();

			}
			return View(p);
		}

		public ActionResult PartialEdit()
		{
			
			return PartialView();
		}


		// POST: AuthorsController/Edit/5
		[HttpPost]
		public ActionResult Edit(Authors p)
		{
			string _query = "UPDATE Authors SET authorName=@authorName, authorBday=@authorBday WHERE authorID=@authorID";
			try
			{
				using (IDbConnection connection = getConnection())
				{

					connection.Execute(_query,p);
				}
					
				return RedirectToAction("Index");
			}
			catch
			{
				return RedirectToAction("Index");
			}
		}

		// GET: AuthorsController/Delete/5
		public ActionResult Delete(int id)
		{
			List<Books> books;
			using (IDbConnection connection = getConnection())
			{
				string selectQuery = "SELECT * FROM Books WHERE bookAuthorID="+id;
				string deleteQuery = "DELETE FROM Authors WHERE authorID=" + id;
				books = connection.Query<Books>(selectQuery).ToList();
				if (books.Count == 0)
				{
					int rows = connection.Execute(deleteQuery);

				}
				return RedirectToAction("Index");
			}
		}

		// POST: AuthorsController/Delete/5
		[HttpPost]
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

		public IDbConnection getConnection()
		{
			IDbConnection temp = new SqlConnection(connectionString);
			return temp;
		}
	}
}
