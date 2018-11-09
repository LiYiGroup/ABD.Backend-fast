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
        public string BUMP_TYPE { get; set; }
        public int NUMBER { get; set; }

        public string UNIT { get; set; }
        public double PRICE { get; set; }
        public double AMOUNT { get; set; }
        public string BUMP_SERIAL_NO { get; set; }
        public int FLOW { get; set; }
        public int LIFT { get; set; }

        public string MATERIAL_BUMP { get; set; }
        public string MATERIAL_FAN { get; set; }
        public string MATERIAL_ROLLER { get; set; }
        public string MOTOR_BRAND { get; set; }
        public string MOTOR_DEMAND { get; set; }

        public string SEAL_FORM { get; set; }
        public string SEAL_BRAND { get; set; }
        public string ROLLER_BRAND { get; set; }
        public string COUPLING { get; set; }
        public string SEAL_COOLER { get; set; }

        public string CAVITATION_ALLOWANCE { get; set; }
        public string ACTUAL_BUMP_SPEED { get; set; }
        public string STATION { get; set; }
        public string TEMPERATURE { get; set; }
        public string DENSITY { get; set; }

        public string IN_PRESSURE { get; set; }
        public string MEDIUM { get; set; }
        public string VISCOSITY { get; set; }
        public string PARTICULATES { get; set; }
        public string WORKING_PRESSURE { get; set; }

        public string FLANGES_STANDARD { get; set; }
        public string FLANGES_LEVEL { get; set; }
        public string COUPLING_HOOD { get; set; }
        public string BASE { get; set; }
        public string ANCHOR_BOLT { get; set; }

        public string PAINT { get; set; }
        public string SURFACE_TREATMENT { get; set; }
        public string PACKAGE { get; set; }
        public string TRANSPORT { get; set; }
    }


    public class ORDER_LIST_DETAIL_DICT_NAME
    {
        public string ORDER_NO { get; set; }
        public string BUMP_ID { get; set; }
        public string BUMP_NM { get; set; }
        public string BUMP_TYPE { get; set; }
        public int NUMBER { get; set; }

        public string UNIT { get; set; }
        public double PRICE { get; set; }
        public double AMOUNT { get; set; }
        public string BUMP_SERIAL_NO { get; set; }
        public int FLOW { get; set; }
        public int LIFT { get; set; }

        public string MATERIAL_BUMP { get; set; }
        public string MATERIAL_FAN { get; set; }
        public string MATERIAL_ROLLER { get; set; }
        public string MOTOR_BRAND { get; set; }
        public string MOTOR_DEMAND { get; set; }

        public string SEAL_FORM { get; set; }
        public string SEAL_BRAND { get; set; }
        public string ROLLER_BRAND { get; set; }
        public string COUPLING { get; set; }
        public string SEAL_COOLER { get; set; }

        public string CAVITATION_ALLOWANCE { get; set; }
        public string ACTUAL_BUMP_SPEED { get; set; }
        public string STATION { get; set; }
        public string TEMPERATURE { get; set; }
        public string DENSITY { get; set; }

        public string IN_PRESSURE { get; set; }
        public string MEDIUM { get; set; }
        public string VISCOSITY { get; set; }
        public string PARTICULATES { get; set; }
        public string WORKING_PRESSURE { get; set; }

        public string FLANGES_STANDARD { get; set; }
        public string FLANGES_LEVEL { get; set; }
        public string COUPLING_HOOD { get; set; }
        public string BASE { get; set; }
        public string ANCHOR_BOLT { get; set; }

        public string PAINT { get; set; }
        public string SURFACE_TREATMENT { get; set; }
        public string PACKAGE { get; set; }
        public string TRANSPORT { get; set; }

        public string MOTOR_BRAND_NAME { get; set; }
        public string SEAL_FORM_NAME { get; set; }
        public string SEAL_BRAND_NAME { get; set; }
        public string ROLLER_BRAND_NAME { get; set; }
        public string COUPLING_NAME { get; set; }
        public string SEAL_COOLER_NAME { get; set; }

        public string FLANGES_STANDARD_NAME { get; set; }
        public string FLANGES_LEVEL_NAME { get; set; }
        public string COUPLING_HOOD_NAME { get; set; }
        public string BASE_NAME { get; set; }
        public string ANCHOR_BOLT_NAME { get; set; }

        public string PAINT_NAME { get; set; }
        public string SURFACE_TREATMENT_NAME { get; set; }
        public string PACKAGE_NAME { get; set; }
        public string TRANSPORT_NAME { get; set; }
    }
}
