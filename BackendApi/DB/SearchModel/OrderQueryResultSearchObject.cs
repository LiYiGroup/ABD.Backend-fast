using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.DB.SearchModel
{
    public class OrderQueryResultSearchObject
    {
        public string SALES_PERSON { get; set; }
        public string ORDER_UNIT { get; set; }
        public string PROJECT_NM { get; set; }
        public DateTime DEPARTURE_DATE_ST { get; set; }
        public DateTime DEPARTURE_DATE_ED { get; set; }
        public DateTime DELIVERY_DATE_ST { get; set; }
        public DateTime DELIVERY_DATE_ED { get; set; }
        public string ORDER_NO { get; set; }
        public int QTY { get; set; }
        public Double TOTAL_AMT { get; set; }
        public Double DISCOUNT { get; set; }
        public int DELIVERY_QTY { get; set; }
        public Double DOC_AMT { get; set; }
        public Double ARRIVAL_AMT { get; set; }
        public Double REMAIN_AMT { get; set; }
    }
}

