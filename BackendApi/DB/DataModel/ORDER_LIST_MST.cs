using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.DB.DataModel
{
    public class ORDER_LIST_MST
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
        public string DEBUG { get; set; }
        public int TOTAL_QTY { get; set; }
        public string TEX_RATE { get; set; }
        public DateTime GUARANTEE_DATE { get; set; }
        public double TOTAL_AMT { get; set; }
        public string PAYMENT { get; set; }
        public double TARGET_PRICE { get; set; }
        public double DISCOUNT { get; set; }
        public string CHANGE_HIS1 { get; set; }
        public string CHANGE_HIS2 { get; set; }
    }
}
