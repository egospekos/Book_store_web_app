using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Layer
{
	public class Categories
	{
		public int categoryID { get; set; }
		public string categoryName { get; set; }
		public ICollection<Books> books { get; set; }

		public int sumCategory { get; set; }
	}
}
