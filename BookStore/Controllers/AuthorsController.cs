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
			
			return View();
		}

		public List<Authors> GetAllAuthors()
		{
			List<Authors> data;
			using (IDbConnection db = getConnection())
			{
				string _query = @"Select * From Authors a LEFT JOIN
					(Select bookAuthorID as authorID,COUNT(*) as countBook FROM Books 
					GROUP BY bookAuthorID) b ON a.authorID = b.authorID ";
				data = db.Query<Authors>(_query).ToList();
			}
			return data;
		}

		public List<Books> GetBooks(int id)
		{
			List<Books> books = new List<Books>();
            if (id != null)
            {
				using (IDbConnection db = getConnection())
				{
					string _query = "Select * From Books WHERE bookAuthorID=@id";
					books = db.Query<Books>(_query, new {id= id }).ToList();
				}

			}
			return books;
		}
		


		// POST: AuthorsController/Create
		[HttpPost]
		public ActionResult Create(Authors p)
		{
			string _query = "INSERT INTO Authors VALUES(@authorName,@authorBday)";
			try
			{
				using (IDbConnection connection = getConnection())
				{
					connection.Execute(_query,p);
				}
					return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
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

		[HttpPost]
		public void Delete(int id)
		{
			List<Books> books;
			using (IDbConnection connection = getConnection())
			{
				
				string deleteQuery = "DELETE FROM Authors WHERE authorID=@id";
				connection.Execute(deleteQuery,new {id=id});
					
				
			}
		}


		public IDbConnection getConnection()
		{
			IDbConnection temp = new SqlConnection(connectionString);
			return temp;
		}
	}
}
