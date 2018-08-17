using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.DB.DataModel
{
    public class ORDER_DETAIL_MST
    {
        public string ORDER_NO { get; set; }
        public string CONTRACT_NO { get; set; }
        public string PROJECT_NM { get; set; }
        public string ORDER_UNIT { get; set; }
        public string SALES_PERSON { get; set; }
        // Null Check is need
        public DateTime DEPARTURE_DATE { get; set; }
        public DateTime DELIVERY_DATE { get; set; }
        public string REMARK { get; set; }
        public string APPLICATION_ENGINEER { get; set; }
    }
}
