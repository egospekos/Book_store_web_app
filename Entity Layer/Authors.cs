using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Layer
{
	public class Authors
	{
		public int authorID { get; set; }
		public String authorName { get; set; }
		public DateTime authorBday { get; set; }

		public int countBook { get; set; }
		public ICollection<Books> books { get; set; }
	}
}
