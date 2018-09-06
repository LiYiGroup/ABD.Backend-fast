using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.DB.DataModel
{
    public class BOM_ITEM_STANDARD
    {
        public string BOM_ID { get; set; }
        public int ITEM_NO { get; set; }
        public string ITEM_NAME { get; set; }
        public string SPEC { get; set; }
        public string QTY { get; set; }
        public string MATERIAL { get; set; }
        public string REMARK { get; set; }
    }
}
