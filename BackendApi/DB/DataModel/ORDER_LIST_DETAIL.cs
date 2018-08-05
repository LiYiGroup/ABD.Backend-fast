using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.DB.DataModel
{
    public class ORDER_LIST_DETAIL
    {
        public string ORDER_NO { get; set; }
        public string BUMP_ID { get; set; }
        public string BUMP_NM { get; set; }
        public string STATION { get; set; }
        public string BUMP_TYPE { get; set; }
        public int NUMBER { get; set; }
        public int FLOW { get; set; }
        public int LIFT { get; set; }
        public string MATERIAL { get; set; }
        public string SEAL { get; set; }
        public string BUMP_SERIAL_NO { get; set; }
        public string STATUS { get; set; }
        public string REMARK { get; set; }
    }
}
