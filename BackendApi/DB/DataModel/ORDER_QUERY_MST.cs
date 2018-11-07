using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.DB.DataModel
{
    public class ORDER_QUERY_MST
    {
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
