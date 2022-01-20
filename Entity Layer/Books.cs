using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Layer
{
	public class Books
	{
		public int bookID { get; set; }
		public string bookName { get; set; }
		public int bookPages { get; set; }
		public int bookPrice { get; set; }
		public int authorID { get; set; }
		public Authors bookAuthor { get; set; }
		public string authorName { get; set; }

		// public ICollection<Categories> categories { get; set; }

		// public int categoryID { get; set; }
		public String[] categoryNames { get; set; }
		public int[] categoryIDs { get; set; }
	}
}
