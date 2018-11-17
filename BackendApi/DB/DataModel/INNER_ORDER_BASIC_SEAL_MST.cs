using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.DB.DataModel
{
    public class INNER_ORDER_BASIC_SEAL_MST
    {
        public string ORDER_NO { get; set; }
        public string BUMP_ID { get; set; }
        public string BUMP_TYPE { get; set; }
        public string BUMP_SERIAL_NO { get; set; }
        public int NUMBER { get; set; }
        public int WORKING_PRESSURE { get; set; }
        public int TEST_PRESSURE { get; set; }
        public int SUPPRESS_PRESSURE { get; set; }
        public string FLANGE_STANDARD { get; set; }
        public int FLOW { get; set; }
        public int LIFT { get; set; }
        public int NPSH { get; set; }
        public int BUMP_SPEED { get; set; }
        public string STATION { get; set; }
        public string MEDIA { get; set; }
        public int TEMPERATURE { get; set; }
        public int VISCOSITY { get; set; }
        public int INLET_PRESSURE { get; set; }
        public string PARTICULATES { get; set; }

        public string SEAL_TYPE { get; set; }
        public string SEAL_MODEL { get; set; }
        public string SEAL_MATERIAL { get; set; }
        public string OTHER_SEAL_PROVIDER { get; set; }
        public string OTHER_SEAL_INFO { get; set; }
        public string OTHER_SEAL_MODEL { get; set; }
        public string NEED_SEAL_COOLER_FLG { get; set; }
        public string SEAL_COOLER_MODEL { get; set; }
        public string BEARING_BRAND { get; set; }
        public string BEARING_OTHER_INFO { get; set; }
        public string INSTALL_DIRECTION { get; set; }
        public string ABD_SEAL_INFO { get; set; }


		//2018-11-13新增字段

		public string MATERIAL_BUMP { get; set; }
		public string MATERIAL_FAN { get; set; }
		public string MATERIAL_ROLLER { get; set; }
		public string SEAL_FORM { get; set; }
		public string SEAL_BRAND { get; set; }
		public string SEAL_BRAND_MANUAL { get; set; }
		public string DENSITY { get; set; }
		public string FLANGE_LEVEL { get; set; }

		public int    DOUBLE_SEAL_PRESSURE { get; set; }		
		public string SERIES { get; set; }
		public string MAIN_SHAFT_FORM { get; set; }
		public string PUMP_STEERING { get; set; }
		public string ASSEMBLE_DIRECTION { get; set; }
		public string LINE_LOCATION_DESCRIPTION { get; set; }
		public string DRAWING_CONFIRM { get; set; }
		public string LUBRICATING_FORM { get; set; }

	}
}
