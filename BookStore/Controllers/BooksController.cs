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
			
			return View();
		}

		[HttpPost]
		public List<Books> GetAllBooks(Filter filter)
        {
			List<Books> Books;
			List<BookCategories> categories;
			using (IDbConnection db = getConnection())
			{
				Books = ApplyFilter(filter);
				// for xml path categories names tek string dön
				// books içinde string categories prop
				// editte kitap için categoryids getir (kitap id ile)

				//foreach (var item in Books)
				//{
				//	categories = db.Query<BookCategories>("Select * From BookCategories JOIN Categories ON BookCategories.categoryID=Categories.categoryID WHERE bookID=@bookID", item).ToList();
				//	item.categoryNames = categories.Select(x => x.categoryName).ToArray();
				//	item.categoryIDs = categories.Select(x => x.categoryID).ToArray();

				//}
			}
			return Books;
		}


		public List<Books> ApplyFilter(Filter filters)
		{
			
			List<Books> Books;
			string _query;
			using (IDbConnection db = getConnection())
			{
				
				_query = "SELECT DISTINCT b.[bookID],b.[bookName],b.[bookPages],b.[bookPrice],b.[bookAuthorID], a.[authorName], a.[authorID] " +
					"FROM [Books] b " +
					"JOIN [BookCategories] bc ON b.bookID=bc.[bookID] " +
					"LEFT JOIN Authors a ON b.bookAuthorID = a.authorID " +
					"WHERE 1=1";

				//_query += PriceFilter(filters.minPrice, filters.maxPrice);
				if (filters.maxPrice != 0)
				{
					_query += " AND [bookPrice]<=@max ";
				}
				if (filters.minPrice != 0)
				{
					_query += " AND [bookPrice]>=@min ";
				}
				if (filters.categoryIDs !=null && filters.categoryIDs.Any())
				{
					_query += " AND bc.[categoryID] IN @categories ";
				}
				//_query += CategoryFilter(filters.categoryIDs);
				Books = db.Query<Books>(_query, new {max=filters.maxPrice,min=filters.minPrice, categories=filters.categoryIDs }).ToList();

			}
			return Books;
		}
		     
		public String PriceFilter(int? min,int? max)
		{
			String s = "";
			if (min != 0 && min != null) s+= " AND [bookPrice]>"+min;
			if (max != 0 && max != null) s+= " AND [bookPrice]<"+max;
			
			return s;
		}
		public String CategoryFilter(int[] categories) 
		{
			String s = "";
			if (categories != null)
			{

				
				s += " AND bc.[categoryID] IN(" + categories[0];
				for (int i = 1; i < categories.Length; i++)
				{
					s += ", " + categories[i];
				}
				s += ")";
			}
			return s;
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

		[HttpPost]
		public ActionResult Create(Books b)
		{
			
			string addBookQuery = "INSERT INTO Books (bookName,bookPages,bookPrice,bookAuthorID) VALUES (@bookName,@bookPages,@bookPrice,@authorID);SELECT @@Identity";
			string addBookCatQuery = "INSERT INTO BookCategories (categoryID,bookID) VALUES (@categoryID,@bookID)";
			try
			{
				int rows, rows_2, newID;
				using (IDbConnection connection = getConnection())
				{
					newID = connection.Query<int>(addBookQuery,b).SingleOrDefault();
					//newID = FindID(b);
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
		
		public String CatString(int catID,int bookID)
		{
			return "INSERT INTO BookCategories (categoryID,bookID) VALUES (" + catID + "," + bookID + ")";
		}


		// POST: BookController/Edit/5
		[HttpPost]
		public void Edit(Books p)
		{
			string _queryUPDATE = "UPDATE Books SET bookName=@bookName, bookPages=@bookPages, bookPrice=@bookPrice, bookAuthorID=@authorID WHERE bookID=@bookID";
			string _queryDropCats = "DELETE FROM BookCategories WHERE bookId=@bookID";
			
			try
			{
				
				using(IDbConnection connection = getConnection())
				{
					connection.Execute(_queryUPDATE,p);
					connection.Execute(_queryDropCats,p);
					foreach (var item in p.categoryIDs)
					{
						connection.Execute("INSERT INTO BookCategories(categoryID, bookID) VALUES(@catID,@bookID)", new {catID=item,bookID=p.bookID});
					}

				}
				
			}

			catch
			{
				Console.WriteLine("Book editleme hatası");
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

		public IDbConnection getConnection()
		{
			IDbConnection temp = new SqlConnection(connectionString);
			return temp;
		}
	}
}
