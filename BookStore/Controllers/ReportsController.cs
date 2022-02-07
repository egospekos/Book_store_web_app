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


			return View();
		}

		public List<Categories> GetFirstChart()
		{
			List<Categories> categoriesNumber;
			using (IDbConnection db = getConnection())
			{
				String _query = @"SELECT a.[categoryName],COUNT(*) AS sumCategory 
								FROM Categories a LEFT JOIN [BookCategories] b 
								ON a.[categoryID] = b.[categoryID] 
								GROUP BY a.[categoryName]";
				categoriesNumber = db.Query<Categories>(_query).ToList();
			}
			return categoriesNumber;
		}

		public List<Authors> GetSecondChart()
		{
			List<Authors> a;
			using (IDbConnection db = getConnection())
			{
				String _query = @"SELECT a.[authorName],COUNT(*) AS countBook 
								FROM [Authors] a LEFT JOIN [Books] b 
								ON a.[authorID]=b.[bookAuthorID] 
								GROUP BY a.[authorName]";
				a = db.Query<Authors>(_query).ToList();
			}
			return a;
		}

	}
}
