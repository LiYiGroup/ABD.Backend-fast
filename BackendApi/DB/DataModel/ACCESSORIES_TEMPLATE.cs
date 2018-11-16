using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.DB.DataModel
{
    public class ACCESSORIES_TEMPLATE
    {
        public int SEQ_ID { get; set; }
        public string ORDER_NO { get; set; }
        public string NAME { get; set; }
        public string SPEC { get; set; }
        public int QTY { get; set; }
        public string UNIT { get; set; }
        public double PRICE { get; set; }
        public double TOTAL_AMT { get; set; }
        public string BUMP_TYPE { get; set; }
        public string REMARK { get; set; }
    }
}
