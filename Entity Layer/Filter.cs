using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Layer
{
	public class Filter
	{
		public int[] categoryIDs { get; set; }
		public int? minPrice { get; set; }
		public int? maxPrice { get; set; }
	}
}
