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
			//int max, min;
			//if(filter.maxPrice!=null) max = int.Parse(filter.maxPrice);
			//else max = -1;
			//if (filter.minPrice != null) min = int.Parse(filter.minPrice);
			//else min = -1;

			int? max, min;
			if (filter.maxPrice != null) max = filter.maxPrice;
			else max = -1;
			if (filter.minPrice != null) min = filter.minPrice;
			else min = -1;


			int[] categories = filter.categoryIDs;
			List<Books> books;

			var sql = @" SELECT DISTINCT b.[bookID],b.[bookName],b.[bookPages],b.[bookPrice],b.[bookAuthorID], a.[authorName], a.[authorID],
						(select STUFF ((
						SELECT ','+c.categoryName FROM BookCategories bc JOIN Categories c ON bc.categoryID = c.categoryID WHERE bc.bookID = b.bookID FOR XML PATH('')),1,1,'')) as categoryNames  
						FROM [Books] b 
						LEFT JOIN Authors a ON b.bookAuthorID = a.authorID 
						LEFT JOIN BookCategories bcc ON bcc.bookID = b.bookID WHERE 1=1 ";
			if (max != -1)
			{
				sql += " AND [bookPrice]<=@max ";
			}
			if (min != -1)
			{
				sql += " AND [bookPrice]>=@min ";
			}
			if (categories != null && categories.Any())
			{
				sql += " AND bcc.[categoryID] IN @categories ";
			}


			//string firstTable = ApplyFilter(filter);
			//string secondTable = "SELECT bookID, categoryNames = STUFF((SELECT ','+CONVERT(VARCHAR(6),categoryID) FROM BookCategories t1 WHERE t1.bookID = t2.bookID FOR XML PATH('')),1,1,'') FROM BookCategories t2 GROUP BY bookID";
			//string fullQuery = "SELECT * FROM ("+firstTable+") tl JOIN ("+secondTable+") tr ON tl.bookID = tr.bookID";
			using (IDbConnection db = getConnection())
			{

				books = db.Query<Books>(sql, new {max= max, min=min,categories= categories }).ToList();
				// connection.Execute(_query, new { catID = item, bookID = newID });
				//newID = connection.Query<int>(addBookQuery,b).SingleOrDefault();
			}
			return books;
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

		public int[] getBookCategories(int id)
		{
			List<BookCategories> bCategories;
			int[] _categories;
			string _query = "SELECT * FROM BookCategories WHERE bookID=@id";
			using (IDbConnection connection = getConnection())
			{

				bCategories = connection.Query<BookCategories>(_query, new {id=id}).ToList();

			}
			_categories = bCategories.Select(x => x.categoryID).ToArray();
			return _categories;


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
			
			string addBookQuery = @"INSERT INTO Books (bookName,bookPages,bookPrice,bookAuthorID) 
								  VALUES (@bookName,@bookPages,@bookPrice,@authorID);SELECT @@Identity";
			
			try
			{
				int newID;
				using (IDbConnection connection = getConnection())
				{
					newID = connection.Query<int>(addBookQuery,b).SingleOrDefault();
					string _query = @"INSERT INTO BookCategories (categoryID,bookID) 
									VALUES (@catID,@bookID)";
					if (b.categoryIDs != null)
					{
						foreach (var item in b.categoryIDs)
						{
							connection.Execute(_query, new { catID = item, bookID = newID });
						}
					}
					
					
				}
					return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
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
					if(p.categoryIDs != null)
					{
						foreach (var item in p.categoryIDs)
						{
							connection.Execute("INSERT INTO BookCategories(categoryID, bookID) VALUES(@catID,@bookID)", new { catID = item, bookID = p.bookID });
						}
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
