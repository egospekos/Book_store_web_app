using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Layer
{
	public class BookCategories
	{
		public int bookID { get; set; }
		public int categoryID { get; set; }
		public Books book { get; set; }
		public Categories category { get; set; }

		public String categoryName { get; set; }
	}
}
