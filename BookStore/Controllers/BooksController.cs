using Dapper;
using Entity_Layer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System.Data;
using System.Data.SqlClient;


namespace BookStore.Controllers
{
	public class BooksController : Controller
	{
		private readonly String connectionString;
		public BooksController(IConfiguration config)
		{
			connectionString = config.GetConnectionString("DefaultConnection");
		}
		// GET: BookController
		public ActionResult Index()
		{
			List<Books> Books;
			List<BookCategories> categories;
			using (IDbConnection db = getConnection())
			{
				Books = db.Query<Books>("Select * From Books LEFT JOIN Authors ON Books.bookAuthorID = Authors.authorID").ToList();

				foreach (var item in Books)
				{
					categories = db.Query<BookCategories>("Select * From BookCategories JOIN Categories ON BookCategories.categoryID=Categories.categoryID WHERE bookID=@bookID", item).ToList();
					item.categoryNames = categories.Select(x=>x.categoryName).ToArray();
					item.categoryIDs = categories.Select(x=>x.categoryID).ToArray();

				}
			}
			return View(Books);
		}


		// GET: BookController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: BookController/Create
		public ActionResult Create()
		{
			List<Categories> categories = getCategories();
			return View(categories);
		}

		
		public List<Categories> getCategories()
		{
			List<Categories> categories;
			string _query = "SELECT * FROM Categories";
			using (IDbConnection connection = getConnection())
			{

				categories = connection.Query<Categories>(_query).ToList();

			}
			return categories;


		}
		public List<Authors> getAuthors()
		{
			List<Authors> authors;
			string _query = "SELECT * FROM Authors";
			using (IDbConnection connection = getConnection())
			{

				authors = connection.Query<Authors>(_query).ToList();

			}
			return authors;


		}

		// POST: BookController/Create
		[HttpPost]
		public ActionResult Create(Books b)
		{
			
			string addBookQuery = "INSERT INTO Books (bookName,bookPages,bookPrice,bookAuthorID) VALUES (@bookName,@bookPages,@bookPrice,@authorID)";
			string addBookCatQuery = "INSERT INTO BookCategories (categoryID,bookID) VALUES (@categoryID,@bookID)";
			try
			{
				int rows, rows_2, newID;
				using (IDbConnection connection = getConnection())
				{
					rows = connection.Execute(addBookQuery,b);
					newID = FindID(b);
					foreach (var item in b.categoryIDs)
					{
						rows_2 = connection.Execute(CatString(item, newID));
					}
					
				}
					return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
		public int FindID(Books b)
		{
			string selectQuery = "SELECT * FROM Books WHERE bookName = @bookName";
			Books temp;
			using (IDbConnection connection = getConnection())
			{
				temp = connection.Query<Books>(selectQuery, b).SingleOrDefault();
				

			}
			return temp.bookID;
		}
		public String CatString(int catID,int bookID)
		{
			return "INSERT INTO BookCategories (categoryID,bookID) VALUES (" + catID + "," + bookID + ")";
		}

		// GET: BookController/Edit/5
		public ActionResult Edit(int id)
		{
			Books b;
			using(IDbConnection connection = getConnection())
			{
				string _query = "SELECT * FROM Books WHERE bookID="+id;
				b = connection.Query<Books>(_query).SingleOrDefault();
			}
			return View(b);
		}

		// POST: BookController/Edit/5
		[HttpPost]
		public ActionResult Edit(int id, Books p)
		{
			string _queryPropChange = "UPDATE Books SET bookName=@bookName, bookPages=@bookPages, bookPrice=@bookPrice, authorID=@bookAuthorID WHERE bookID="+id;
			string _queryDropCats = "DELETE FROM BookCategories WHERE bookId=@bookID";
			string _queryNewCats = "INSERT INTO BookCategories (categoryID,bookID) VALUES (@categoryID,@bookID)";
			try
			{
				int rows, rows_2,rows_3;
				using(IDbConnection connection = getConnection())
				{
					rows = connection.Execute(_queryPropChange,p);
					rows_2 = connection.Execute(_queryDropCats,p);
					foreach (var item in p.categoryIDs)
					{
						rows_3 = connection.Execute(CatString(item,p.bookID));
					}

				}
				return RedirectToAction(nameof(Index));
			}

			catch
			{
				return View();
			}
		}


		// GET: BookController/Delete/5
		public ActionResult Delete(int id)
		{
			string _categoryQuery = "DELETE FROM BookCategories WHERE bookID=" + id;
			string _query = "DELETE FROM Books WHERE bookID=" + id;
			using (IDbConnection connection = getConnection())
			{
				int _rows = connection.Execute(_categoryQuery);
				int rows = connection.Execute(_query);
			}
			return RedirectToAction(nameof(Index));
		}

		// POST: BookController/Delete/5
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
