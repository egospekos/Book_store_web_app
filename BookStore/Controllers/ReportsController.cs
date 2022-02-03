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
				String _query = "SELECT [Categories].[categoryName],COUNT(*) AS sumCategory FROM Categories LEFT JOIN [BookCategories] ON [Categories].[categoryID] = [BookCategories].[categoryID] GROUP BY [Categories].[categoryName]";
				categoriesNumber = db.Query<Categories>(_query).ToList();
			}
			return categoriesNumber;
		}

		public List<Authors> GetSecondChart()
		{
			List<Authors> a;
			using (IDbConnection db = getConnection())
			{
				String _query = "SELECT [Authors].[authorName],COUNT(*) AS countBook FROM [Authors] LEFT JOIN [Books] ON [Authors].[authorID]=[Books].[bookAuthorID] GROUP BY [Authors].[authorName]";
				a = db.Query<Authors>(_query).ToList();
			}
			return a;
		}

	}
}
