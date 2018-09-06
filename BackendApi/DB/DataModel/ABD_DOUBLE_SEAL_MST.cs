using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.DB.DataModel
{
	public class ABD_DOUBLE_SEAL_MST
	{
		public string MEDIUM_END_MOVABLE_RING { get; set; }
		public string MEDIUM_END_STATIC_RING { get; set; }
		public string ATMOSPHERIC_END_MOVABLE_RING { get; set; }
		public string ATMOSPHERIC_END_STATIC_RING { get; set; }
		public string SPRING { get; set; }
		public string METAL_BASE { get; set; }
		public string O_TYPE_RING { get; set; }
		public int ID { get; set; }
	}
}