using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.DB.SearchModel
{
    public class OrderListSearchObject
    {
        public string ORDER_NO { get; set; }
        public string CONTRACT_NO { get; set; }
        public string PROJECT_NM { get; set; }
        public string ORDER_UNIT { get; set; }
        public string SALES_PERSON { get; set; }
        public DateTime DEPARTURE_DATE_ST { get; set; }
        public DateTime DEPARTURE_DATE_ED { get; set; }
        public DateTime DELIVERY_DATE_ST { get; set; }
        public DateTime DELIVERY_DATE_ED { get; set; }
    }
}
