using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.DB.DataModel
{
    public class INNER_ORDER_OTHER_COMPONENT_MST
    {
        public string ORDER_NO { get; set; }
        public string BUMP_ID { get; set; }

        // 底座
        public string BASE_TYPE { get; set; }
        public string BASE_SPEC { get; set; }
        public string SPECIAL_BASE_DETAIL { get; set; }
        // 联轴器罩
        public string COUPLING_HOOD_TYPE { get; set; }
        public string COUPLING_HOOD_SPEC { get; set; }
        public string SPECIAL_HOOD_TYPE_DETAIL { get; set; }
        // 地脚螺栓
        public string ANCHOR_BOLT_TYPE { get; set; }
        public string ANCHOR_BOLT_SPEC { get; set; }
        public string ANCHOR_BOLT_MATERIAL { get; set; }
        public string ANCHOR_BOLT_EXTRA_NUT_SPEC { get; set; }
        public string ANCHOR_BOLT_EXTRA_PAD_SPEC { get; set; }
        public int ANCHOR_BOLT_NUM { get; set; }
        // 联轴器
        public string COUPLING_TYPE { get; set; }
        public string COUPLING_BUMP_COUPLET { get; set; }
        public string COUPLING_ELECTRIC_COUPLET { get; set; }
        public string COUPLING_PIN { get; set; }
        public string COUPLING_JUMP_RING { get; set; }
        public string COUPLING_PROVIDER { get; set; }
        public string COUPLING_SPEC { get; set; }
        public int COUPLING_NUM { get; set; }
        public string SPECIAL_COUPLING_TYPE_DETAIL { get; set; }
        // 电机
        public string ELECTRIC_MOTER_TYPE { get; set; }
        public string ELECTRIC_MOTER_PROVIDER { get; set; }
        public int ELECTRIC_MOTER_POWER { get; set; }
        public int ELECTRIC_MOTER_SPEED { get; set; }
        public string ELECTRIC_MOTER_PFV { get; set; }
        public string ELECTRIC_MOTER_EXTRA_INFO { get; set; }
        // 油漆及外表颜色
        public string COLOR_TYPE { get; set; }
        public string SPECIAL_COLOR_DETAIL { get; set; }
        public string SURFACE_TREAT_TYPE { get; set; }
        public string SURFACE_TREAT_EXTRA_INFO { get; set; }
        // 包装及运输
        public string TRANSPORT_TYPE { get; set; }
        public string TRANSPORT_PLACE { get; set; }
        public string PACKAGING_TYPE { get; set; }
        public string SPECIAL_PACK_DETAIL { get; set; }
        public string NEED_FUME_CERTIFICATE { get; set; }
        public string ADDRESS_INFO { get; set; }
        public string REMARK { get; set; }
    }
}
