using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BackendApi.DB;
using BackendApi.DB.DataModel;
using BackendApi.DB.SearchModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InnerOrderController : ControllerBase
    {
        private readonly ABD_DbContext myContext;

        public InnerOrderController(ABD_DbContext context)
        {
            myContext = context;
        }
        // GET api/innerOrder/existBumpInfo/{BUMP_INFO}
        [HttpGet("existBumpInfo/{BUMP_INFO}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Get(string BUMP_INFO)
        {
            String returnJson = String.Empty;
            if (!String.IsNullOrEmpty(BUMP_INFO))
            {
                BUMP_INFO = BUMP_INFO.Replace("|SLASH|", "/");
                string[] ORDER_NO_AND_BUMP_IDS = BUMP_INFO.Split("|DASH|");
                string BUMP_ID_T = String.Empty;
                string ORDER_NO_T = String.Empty;

                if (ORDER_NO_AND_BUMP_IDS.Length > 1)
                {
                    ORDER_NO_T = ORDER_NO_AND_BUMP_IDS[0];
                    BUMP_ID_T = ORDER_NO_AND_BUMP_IDS[1];
                    var tempObj = JsonConvert.SerializeObject(myContext. ORDER_LIST_DETAIL
                                    .Where(d => d.ORDER_NO == ORDER_NO_T)
                                    .Where(d => d.BUMP_ID == BUMP_ID_T));
                    if (tempObj != "[]")
                    {
                        var orderListDetailEntity = from a in myContext.ORDER_LIST_DETAIL
                                                    join b in myContext.M_DICT
                                                    on a.MOTOR_BRAND equals b.DICT_ID into b1
                                                    from b2 in b1.DefaultIfEmpty()

                                                    join c in myContext.M_DICT
                                                    on a.SEAL_FORM equals c.DICT_ID into c1
                                                    from c2 in c1.DefaultIfEmpty()

                                                    join d in myContext.M_DICT
                                                    on a.SEAL_BRAND equals d.DICT_ID into d1
                                                    from d2 in d1.DefaultIfEmpty()

                                                    join e in myContext.M_DICT
                                                    on a.ROLLER_BRAND equals e.DICT_ID into e1
                                                    from e2 in e1.DefaultIfEmpty()

                                                    join f in myContext.M_DICT
                                                    on a.COUPLING equals f.DICT_ID into f1
                                                    from f2 in f1.DefaultIfEmpty()

                                                    join g in myContext.M_DICT
                                                    on a.SEAL_COOLER equals g.DICT_ID into g1
                                                    from g2 in g1.DefaultIfEmpty()

                                                    join h in myContext.M_DICT
                                                    on a.FLANGES_STANDARD equals h.DICT_ID into h1
                                                    from h2 in h1.DefaultIfEmpty()

                                                    join i in myContext.M_DICT
                                                    on a.FLANGES_LEVEL equals i.DICT_ID into i1
                                                    from i2 in i1.DefaultIfEmpty()

                                                    join j in myContext.M_DICT
                                                    on a.COUPLING_HOOD equals j.DICT_ID into j1
                                                    from j2 in j1.DefaultIfEmpty()

                                                    join k in myContext.M_DICT
                                                    on a.BASE equals k.DICT_ID into k1
                                                    from k2 in k1.DefaultIfEmpty()

                                                    join l in myContext.M_DICT
                                                    on a.ANCHOR_BOLT equals l.DICT_ID into l1
                                                    from l2 in l1.DefaultIfEmpty()

                                                    join m in myContext.M_DICT
                                                    on a.PAINT equals m.DICT_ID into m1
                                                    from m2 in m1.DefaultIfEmpty()

                                                    join n in myContext.M_DICT
                                                    on a.SURFACE_TREATMENT equals n.DICT_ID into n1
                                                    from n2 in n1.DefaultIfEmpty()

                                                    join o in myContext.M_DICT
                                                    on a.PACKAGE equals o.DICT_ID into o1
                                                    from o2 in o1.DefaultIfEmpty()

                                                    join p in myContext.M_DICT
                                                    on a.TRANSPORT equals p.DICT_ID into p1
                                                    from p2 in p1.DefaultIfEmpty()

                                                    select new ORDER_LIST_DETAIL_DICT_NAME
                                                    {
                                                        ORDER_NO = a.ORDER_NO,
                                                        BUMP_ID = a.BUMP_ID,
                                                        BUMP_NM = a.BUMP_NM,
                                                        BUMP_TYPE = a.BUMP_TYPE,
                                                        NUMBER = a.NUMBER,

                                                        UNIT = a.UNIT,
                                                        PRICE = a.PRICE,
                                                        AMOUNT = a.AMOUNT,
                                                        BUMP_SERIAL_NO = a.BUMP_SERIAL_NO,
                                                        FLOW = a.FLOW,
                                                        LIFT = a.LIFT,

                                                        MATERIAL_BUMP = a.MATERIAL_BUMP,
                                                        MATERIAL_FAN = a.MATERIAL_FAN,
                                                        MATERIAL_ROLLER = a.MATERIAL_ROLLER,
                                                        MOTOR_BRAND = a.MOTOR_BRAND,
                                                        MOTOR_DEMAND = a.MOTOR_DEMAND,

                                                        SEAL_FORM = a.SEAL_FORM,
                                                        SEAL_BRAND = a.SEAL_BRAND,
                                                        ROLLER_BRAND = a.ROLLER_BRAND,
                                                        COUPLING = a.COUPLING,
                                                        SEAL_COOLER = a.SEAL_COOLER,

                                                        CAVITATION_ALLOWANCE = a.CAVITATION_ALLOWANCE,
                                                        ACTUAL_BUMP_SPEED = a.ACTUAL_BUMP_SPEED,
                                                        STATION = a.STATION,
                                                        TEMPERATURE = a.TEMPERATURE,
                                                        DENSITY = a.DENSITY,

                                                        IN_PRESSURE = a.IN_PRESSURE,
                                                        MEDIUM = a.MEDIUM,
                                                        VISCOSITY = a.VISCOSITY,
                                                        PARTICULATES = a.PARTICULATES,
                                                        WORKING_PRESSURE = a.WORKING_PRESSURE,

                                                        FLANGES_STANDARD = a.FLANGES_STANDARD,
                                                        FLANGES_LEVEL = a.FLANGES_LEVEL,
                                                        COUPLING_HOOD = a.COUPLING_HOOD,
                                                        BASE = a.BASE,
                                                        ANCHOR_BOLT = a.ANCHOR_BOLT,

                                                        PAINT = a.PAINT,
                                                        SURFACE_TREATMENT = a.SURFACE_TREATMENT,
                                                        PACKAGE = a.PACKAGE,
                                                        TRANSPORT = a.TRANSPORT,

                                                        MOTOR_BRAND_NAME = String.IsNullOrEmpty(b2.DICT_NAME) ? a.MOTOR_BRAND : b2.DICT_NAME,
                                                        SEAL_FORM_NAME = String.IsNullOrEmpty(c2.DICT_NAME) ? a.SEAL_FORM : c2.DICT_NAME,
                                                        SEAL_BRAND_NAME = String.IsNullOrEmpty(d2.DICT_NAME) ? a.SEAL_BRAND : d2.DICT_NAME,
                                                        ROLLER_BRAND_NAME = String.IsNullOrEmpty(e2.DICT_NAME) ? a.ROLLER_BRAND: e2.DICT_NAME,
                                                        COUPLING_NAME = String.IsNullOrEmpty(f2.DICT_NAME) ? a.COUPLING : f2.DICT_NAME,
                                                        SEAL_COOLER_NAME = String.IsNullOrEmpty(g2.DICT_NAME) ? a.SEAL_COOLER: g2.DICT_NAME,

                                                        FLANGES_STANDARD_NAME = String.IsNullOrEmpty(h2.DICT_NAME) ? a.FLANGES_STANDARD : h2.DICT_NAME,
                                                        FLANGES_LEVEL_NAME = String.IsNullOrEmpty(i2.DICT_NAME) ? a.FLANGES_LEVEL : i2.DICT_NAME,
                                                        COUPLING_HOOD_NAME = String.IsNullOrEmpty(j2.DICT_NAME) ? a.COUPLING_HOOD : j2.DICT_NAME,
                                                        BASE_NAME = String.IsNullOrEmpty(k2.DICT_NAME) ? a.BASE : k2.DICT_NAME,
                                                        ANCHOR_BOLT_NAME = String.IsNullOrEmpty(l2.DICT_NAME) ? a.ANCHOR_BOLT : l2.DICT_NAME,

                                                        PAINT_NAME = String.IsNullOrEmpty(m2.DICT_NAME) ? a.PAINT : m2.DICT_NAME,
                                                        SURFACE_TREATMENT_NAME = String.IsNullOrEmpty(n2.DICT_NAME) ? a.SURFACE_TREATMENT : n2.DICT_NAME,
                                                        PACKAGE_NAME = String.IsNullOrEmpty(o2.DICT_NAME) ? a.PACKAGE : o2.DICT_NAME,
                                                        TRANSPORT_NAME = String.IsNullOrEmpty(p2.DICT_NAME) ? a.TRANSPORT: p2.DICT_NAME,

                                                        SEAL_BRAND_MANUAL = String.IsNullOrEmpty(a.SEAL_BRAND_MANUAL) ? "" : a.SEAL_BRAND_MANUAL,
                                                        SEAL_BRAND_SHOW = d2.DICT_NAME + (String.IsNullOrEmpty(a.SEAL_BRAND_MANUAL) ? "" : "-" + a.SEAL_BRAND_MANUAL),   //手动添加显示内容，否则显示数据字典名称
                                                    };

                        returnJson = JsonConvert.SerializeObject(orderListDetailEntity
                                    .Where(d => d.ORDER_NO == ORDER_NO_T)
                                    .Where(d => d.BUMP_ID == BUMP_ID_T).First());
                    }
                }
            }
            return returnJson;
        }

        // GET api/innerOrder/basicSealInfo/{BUMP_INFO}
        [HttpGet("basicSealInfo/{BUMP_INFO}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> GetBasicSealInfo(string BUMP_INFO)
        {
            String returnJson = String.Empty;
            if (!String.IsNullOrEmpty(BUMP_INFO))
            {
                BUMP_INFO = BUMP_INFO.Replace("|SLASH|", "/");
                string[] ORDER_NO_AND_BUMP_IDS = BUMP_INFO.Split("|DASH|");
                string BUMP_ID_T = String.Empty;
                string ORDER_NO_T = String.Empty;

                if (ORDER_NO_AND_BUMP_IDS.Length > 1)
                {
                    ORDER_NO_T = ORDER_NO_AND_BUMP_IDS[0];
                    BUMP_ID_T = ORDER_NO_AND_BUMP_IDS[1];
                    var tempObj = JsonConvert.SerializeObject(myContext.INNER_ORDER_BASIC_SEAL_MST
                                    .Where(d => d.ORDER_NO == ORDER_NO_T)
                                    .Where(d => d.BUMP_ID == BUMP_ID_T));
                    if (tempObj != "[]")
                    {
                        returnJson = JsonConvert.SerializeObject(myContext.INNER_ORDER_BASIC_SEAL_MST
                                    .Where(d => d.ORDER_NO == ORDER_NO_T)
                                    .Where(d => d.BUMP_ID == BUMP_ID_T).First());
                    }
                }
            }
            return returnJson;
        }

        // GET api/innerOrder/otherComponentInfo/{BUMP_INFO}
        [HttpGet("otherComponentInfo/{BUMP_INFO}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> GetOtherComponentInfo(string BUMP_INFO)
        {
            String returnJson = String.Empty;
            if (!String.IsNullOrEmpty(BUMP_INFO))
            {
                BUMP_INFO = BUMP_INFO.Replace("|SLASH|", "/");
                string[] ORDER_NO_AND_BUMP_IDS = BUMP_INFO.Split("|DASH|");
                string BUMP_ID_T = String.Empty;
                string ORDER_NO_T = String.Empty;

                if (ORDER_NO_AND_BUMP_IDS.Length > 1)
                {
                    ORDER_NO_T = ORDER_NO_AND_BUMP_IDS[0];
                    BUMP_ID_T = ORDER_NO_AND_BUMP_IDS[1];
                    var tempObj = JsonConvert.SerializeObject(myContext.INNER_ORDER_OTHER_COMPONENT_MST
                                    .Where(d => d.ORDER_NO == ORDER_NO_T)
                                    .Where(d => d.BUMP_ID == BUMP_ID_T));
                    if (tempObj != "[]")
                    {
                        returnJson = JsonConvert.SerializeObject(myContext.INNER_ORDER_OTHER_COMPONENT_MST
                                    .Where(d => d.ORDER_NO == ORDER_NO_T)
                                    .Where(d => d.BUMP_ID == BUMP_ID_T).First());
                    }
                }
            }
            return returnJson;
        }

        // POST api/innerOrder/
        [HttpPost]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Post([FromBody]JObject postObj)
        {
            // FROM WEB PAGE
            ORDER_LIST_DETAIL_DICT_NAME orderListDetailPostEntity = postObj.GetValue("orderListDetailTableModel").ToObject<ORDER_LIST_DETAIL_DICT_NAME>();
            INNER_ORDER_BASIC_SEAL_MST innerOrderBasicSealPostEntity = postObj.GetValue("basicAndSealModel").ToObject<INNER_ORDER_BASIC_SEAL_MST>();
            INNER_ORDER_OTHER_COMPONENT_MST innerOrderOtherComponentPostEntity = postObj.GetValue("otherComponentModel").ToObject<INNER_ORDER_OTHER_COMPONENT_MST>();

            IList innerOrderBOMItemStandard = new ArrayList();
            IList innerOrderBOMItemBase = new ArrayList();

            if (postObj.GetValue("componentListTableData") != null)
            {
                innerOrderBOMItemStandard = postObj.GetValue("componentListTableData").ToList();
            }

            if (postObj.GetValue("basicPartListTableData") != null)
            {
                innerOrderBOMItemBase = postObj.GetValue("basicPartListTableData").ToList();
            }

            // 校验主键
            if (String.IsNullOrEmpty(orderListDetailPostEntity.ORDER_NO) || String.IsNullOrEmpty(orderListDetailPostEntity.BUMP_ID))
            {
                return "";
            }

            // FROM DB
            // 登录BOM的GRID部分，先用SQL删除对应ID项目，之后批量插入
            myContext.Database.ExecuteSqlCommand("DELETE FROM \"INNER_ORDER_BOM_ITEM_BASE\" WHERE \"ORDER_NO\" = @ORDER_NO AND \"BUMP_ID\" = @BUMP_ID", 
                        new NpgsqlParameter("@ORDER_NO", orderListDetailPostEntity.ORDER_NO), new NpgsqlParameter("@BUMP_ID", orderListDetailPostEntity.BUMP_ID));
            myContext.Database.ExecuteSqlCommand("DELETE FROM \"INNER_ORDER_BOM_ITEM_STANDARD\" WHERE \"ORDER_NO\" = @ORDER_NO AND \"BUMP_ID\" = @BUMP_ID",
                        new NpgsqlParameter("@ORDER_NO", orderListDetailPostEntity.ORDER_NO), new NpgsqlParameter("@BUMP_ID", orderListDetailPostEntity.BUMP_ID));
            foreach (JObject item in innerOrderBOMItemStandard)
            {
                INNER_ORDER_BOM_ITEM_STANDARD itemEntity = item.ToObject<INNER_ORDER_BOM_ITEM_STANDARD>();
                itemEntity.ORDER_NO = orderListDetailPostEntity.ORDER_NO;
                itemEntity.BUMP_ID = orderListDetailPostEntity.BUMP_ID;
                myContext.INNER_ORDER_BOM_ITEM_STANDARD.AddAsync(itemEntity);
            }
            foreach (JObject item in innerOrderBOMItemBase)
            {
                INNER_ORDER_BOM_ITEM_BASE itemEntity = item.ToObject<INNER_ORDER_BOM_ITEM_BASE>();
                itemEntity.ORDER_NO = orderListDetailPostEntity.ORDER_NO;
                itemEntity.BUMP_ID = orderListDetailPostEntity.BUMP_ID;
                myContext.INNER_ORDER_BOM_ITEM_BASE.AddAsync(itemEntity);
            }
            myContext.SaveChanges();

            var innerOrderbBasicSealMstEntity = myContext.INNER_ORDER_BASIC_SEAL_MST
                .Where(d => d.ORDER_NO.Equals(orderListDetailPostEntity.ORDER_NO) && d.BUMP_ID.Equals(orderListDetailPostEntity.BUMP_ID));

            var innerOrderOtherComponentMstEntity = myContext.INNER_ORDER_OTHER_COMPONENT_MST
                .Where(d => d.ORDER_NO.Equals(orderListDetailPostEntity.ORDER_NO) && d.BUMP_ID.Equals(orderListDetailPostEntity.BUMP_ID));

            if (innerOrderbBasicSealMstEntity.Count() == 0 && innerOrderOtherComponentMstEntity.Count() == 0)
            {
                // 补充项目
                innerOrderBasicSealPostEntity.ORDER_NO = orderListDetailPostEntity.ORDER_NO;
                innerOrderBasicSealPostEntity.BUMP_ID = orderListDetailPostEntity.BUMP_ID;
                innerOrderBasicSealPostEntity.BUMP_TYPE = orderListDetailPostEntity.BUMP_TYPE;
                innerOrderBasicSealPostEntity.BUMP_SERIAL_NO = orderListDetailPostEntity.BUMP_SERIAL_NO;
                innerOrderBasicSealPostEntity.NUMBER = orderListDetailPostEntity.NUMBER;
                innerOrderBasicSealPostEntity.FLOW = orderListDetailPostEntity.FLOW;
                innerOrderBasicSealPostEntity.LIFT = orderListDetailPostEntity.LIFT;
                innerOrderBasicSealPostEntity.STATION = orderListDetailPostEntity.STATION;
                innerOrderBasicSealPostEntity.MATERIAL_BUMP = orderListDetailPostEntity.MATERIAL_BUMP;
                innerOrderBasicSealPostEntity.MATERIAL_FAN = orderListDetailPostEntity.MATERIAL_FAN;
                innerOrderBasicSealPostEntity.MATERIAL_ROLLER = orderListDetailPostEntity.MATERIAL_ROLLER;
                innerOrderBasicSealPostEntity.SEAL_FORM = orderListDetailPostEntity.SEAL_FORM_NAME;
                innerOrderBasicSealPostEntity.SEAL_BRAND = orderListDetailPostEntity.SEAL_BRAND_NAME;
                innerOrderBasicSealPostEntity.SEAL_BRAND_MANUAL = orderListDetailPostEntity.SEAL_BRAND_MANUAL;
                innerOrderBasicSealPostEntity.NPSH = orderListDetailPostEntity.CAVITATION_ALLOWANCE;
                innerOrderBasicSealPostEntity.BUMP_SPEED = orderListDetailPostEntity.ACTUAL_BUMP_SPEED;
                innerOrderBasicSealPostEntity.TEMPERATURE= orderListDetailPostEntity.TEMPERATURE;
                innerOrderBasicSealPostEntity.DENSITY = orderListDetailPostEntity.DENSITY;
                innerOrderBasicSealPostEntity.INLET_PRESSURE = orderListDetailPostEntity.IN_PRESSURE;
                innerOrderBasicSealPostEntity.MEDIA = orderListDetailPostEntity.MEDIUM;
                innerOrderBasicSealPostEntity.VISCOSITY= orderListDetailPostEntity.VISCOSITY;
                innerOrderBasicSealPostEntity.PARTICULATES = orderListDetailPostEntity.PARTICULATES;
                innerOrderBasicSealPostEntity.WORKING_PRESSURE = orderListDetailPostEntity.WORKING_PRESSURE;
                innerOrderBasicSealPostEntity.FLANGE_STANDARD= orderListDetailPostEntity.FLANGES_STANDARD_NAME;
                innerOrderBasicSealPostEntity.FLANGE_LEVEL = orderListDetailPostEntity.FLANGES_LEVEL_NAME;
                innerOrderBasicSealPostEntity.BEARING_BRAND = orderListDetailPostEntity.ROLLER_BRAND_NAME;
                innerOrderBasicSealPostEntity.NEED_SEAL_COOLER_FLG = orderListDetailPostEntity.SEAL_COOLER_NAME;


                innerOrderOtherComponentPostEntity.ORDER_NO = orderListDetailPostEntity.ORDER_NO;
                innerOrderOtherComponentPostEntity.BUMP_ID = orderListDetailPostEntity.BUMP_ID;

                innerOrderOtherComponentPostEntity.ELECTRIC_MOTER_TYPE = orderListDetailPostEntity.MOTOR_BRAND_NAME;
                innerOrderOtherComponentPostEntity.COUPLING_TYPE = orderListDetailPostEntity.COUPLING_NAME;
                innerOrderOtherComponentPostEntity.BASE_TYPE = orderListDetailPostEntity.BASE_NAME;
                innerOrderOtherComponentPostEntity.COUPLING_HOOD_TYPE = orderListDetailPostEntity.COUPLING_HOOD_NAME;
                innerOrderOtherComponentPostEntity.ANCHOR_BOLT_TYPE = orderListDetailPostEntity.ANCHOR_BOLT_NAME;
                innerOrderOtherComponentPostEntity.COLOR_TYPE = orderListDetailPostEntity.PAINT_NAME;
                innerOrderOtherComponentPostEntity.SURFACE_TREAT_TYPE = orderListDetailPostEntity.SURFACE_TREATMENT_NAME;
                innerOrderOtherComponentPostEntity.PACKAGING_TYPE = orderListDetailPostEntity.PACKAGE_NAME;
                innerOrderOtherComponentPostEntity.TRANSPORT_TYPE = orderListDetailPostEntity.TRANSPORT_NAME;

                // TODO

                myContext.INNER_ORDER_OTHER_COMPONENT_MST.Add(innerOrderOtherComponentPostEntity);
                myContext.INNER_ORDER_BASIC_SEAL_MST.Add(innerOrderBasicSealPostEntity);
                myContext.SaveChanges();
                return "";
            }
            else
            {
                // 补充项目
                innerOrderbBasicSealMstEntity.First().BUMP_TYPE = orderListDetailPostEntity.BUMP_TYPE;
                innerOrderbBasicSealMstEntity.First().BUMP_SERIAL_NO = orderListDetailPostEntity.BUMP_SERIAL_NO;
                innerOrderbBasicSealMstEntity.First().NUMBER = orderListDetailPostEntity.NUMBER;
                innerOrderbBasicSealMstEntity.First().FLOW = orderListDetailPostEntity.FLOW;
                innerOrderbBasicSealMstEntity.First().LIFT = orderListDetailPostEntity.LIFT;
                innerOrderbBasicSealMstEntity.First().STATION = orderListDetailPostEntity.STATION;
                innerOrderbBasicSealMstEntity.First().MATERIAL_BUMP = orderListDetailPostEntity.MATERIAL_BUMP;
                innerOrderbBasicSealMstEntity.First().MATERIAL_FAN = orderListDetailPostEntity.MATERIAL_FAN;
                innerOrderbBasicSealMstEntity.First().MATERIAL_ROLLER = orderListDetailPostEntity.MATERIAL_ROLLER;
                innerOrderbBasicSealMstEntity.First().SEAL_FORM = orderListDetailPostEntity.SEAL_FORM_NAME;
                innerOrderbBasicSealMstEntity.First().SEAL_BRAND = orderListDetailPostEntity.SEAL_BRAND_NAME;
                innerOrderbBasicSealMstEntity.First().SEAL_BRAND_MANUAL = orderListDetailPostEntity.SEAL_BRAND_MANUAL;
                innerOrderbBasicSealMstEntity.First().NPSH = orderListDetailPostEntity.CAVITATION_ALLOWANCE;
                innerOrderbBasicSealMstEntity.First().BUMP_SPEED = orderListDetailPostEntity.ACTUAL_BUMP_SPEED;
                innerOrderbBasicSealMstEntity.First().TEMPERATURE = orderListDetailPostEntity.TEMPERATURE;
                innerOrderbBasicSealMstEntity.First().DENSITY = orderListDetailPostEntity.DENSITY;
                innerOrderbBasicSealMstEntity.First().INLET_PRESSURE = orderListDetailPostEntity.IN_PRESSURE;
                innerOrderbBasicSealMstEntity.First().MEDIA = orderListDetailPostEntity.MEDIUM;
                innerOrderbBasicSealMstEntity.First().VISCOSITY = orderListDetailPostEntity.VISCOSITY;
                innerOrderbBasicSealMstEntity.First().PARTICULATES = orderListDetailPostEntity.PARTICULATES;
                innerOrderbBasicSealMstEntity.First().WORKING_PRESSURE = orderListDetailPostEntity.WORKING_PRESSURE;
                innerOrderbBasicSealMstEntity.First().FLANGE_STANDARD = orderListDetailPostEntity.FLANGES_STANDARD_NAME;
                innerOrderbBasicSealMstEntity.First().FLANGE_LEVEL = orderListDetailPostEntity.FLANGES_LEVEL_NAME;
                innerOrderbBasicSealMstEntity.First().BEARING_BRAND = orderListDetailPostEntity.ROLLER_BRAND_NAME;
                innerOrderbBasicSealMstEntity.First().NEED_SEAL_COOLER_FLG = orderListDetailPostEntity.SEAL_COOLER_NAME;

                innerOrderOtherComponentMstEntity.First().ELECTRIC_MOTER_TYPE = orderListDetailPostEntity.MOTOR_BRAND_NAME;
                innerOrderOtherComponentMstEntity.First().COUPLING_TYPE = orderListDetailPostEntity.COUPLING_NAME;
                innerOrderOtherComponentMstEntity.First().BASE_TYPE = orderListDetailPostEntity.BASE_NAME;
                innerOrderOtherComponentMstEntity.First().COUPLING_HOOD_TYPE = orderListDetailPostEntity.COUPLING_HOOD_NAME;
                innerOrderOtherComponentMstEntity.First().ANCHOR_BOLT_TYPE = orderListDetailPostEntity.ANCHOR_BOLT_NAME;
                innerOrderOtherComponentMstEntity.First().COLOR_TYPE = orderListDetailPostEntity.PAINT_NAME;
                innerOrderOtherComponentMstEntity.First().SURFACE_TREAT_TYPE = orderListDetailPostEntity.SURFACE_TREATMENT_NAME;
                innerOrderOtherComponentMstEntity.First().PACKAGING_TYPE = orderListDetailPostEntity.PACKAGE_NAME;
                innerOrderOtherComponentMstEntity.First().TRANSPORT_TYPE = orderListDetailPostEntity.TRANSPORT_NAME;

                // 其他项目

                innerOrderbBasicSealMstEntity.First().TEST_PRESSURE = innerOrderBasicSealPostEntity.TEST_PRESSURE;
                innerOrderbBasicSealMstEntity.First().SUPPRESS_PRESSURE = innerOrderBasicSealPostEntity.SUPPRESS_PRESSURE;
                innerOrderbBasicSealMstEntity.First().SEAL_TYPE = innerOrderBasicSealPostEntity.SEAL_TYPE;
                innerOrderbBasicSealMstEntity.First().SEAL_MODEL = innerOrderBasicSealPostEntity.SEAL_MODEL;
                innerOrderbBasicSealMstEntity.First().SEAL_MATERIAL = innerOrderBasicSealPostEntity.SEAL_MATERIAL;
                innerOrderbBasicSealMstEntity.First().ABD_SEAL_INFO = innerOrderBasicSealPostEntity.ABD_SEAL_INFO;
                innerOrderbBasicSealMstEntity.First().OTHER_SEAL_PROVIDER = innerOrderBasicSealPostEntity.OTHER_SEAL_PROVIDER;
                innerOrderbBasicSealMstEntity.First().OTHER_SEAL_INFO = innerOrderBasicSealPostEntity.OTHER_SEAL_INFO;
                innerOrderbBasicSealMstEntity.First().OTHER_SEAL_MODEL = innerOrderBasicSealPostEntity.OTHER_SEAL_MODEL;
                innerOrderbBasicSealMstEntity.First().NEED_SEAL_COOLER_FLG = innerOrderBasicSealPostEntity.NEED_SEAL_COOLER_FLG;
                innerOrderbBasicSealMstEntity.First().SEAL_COOLER_MODEL = innerOrderBasicSealPostEntity.SEAL_COOLER_MODEL;
                innerOrderbBasicSealMstEntity.First().BEARING_BRAND = innerOrderBasicSealPostEntity.BEARING_BRAND;
                innerOrderbBasicSealMstEntity.First().BEARING_OTHER_INFO = innerOrderBasicSealPostEntity.BEARING_OTHER_INFO;
                innerOrderbBasicSealMstEntity.First().INSTALL_DIRECTION = innerOrderBasicSealPostEntity.INSTALL_DIRECTION;
                innerOrderbBasicSealMstEntity.First().DOUBLE_SEAL_PRESSURE = innerOrderBasicSealPostEntity.DOUBLE_SEAL_PRESSURE;
                innerOrderbBasicSealMstEntity.First().SERIES = innerOrderBasicSealPostEntity.SERIES;
                innerOrderbBasicSealMstEntity.First().MAIN_SHAFT_FORM = innerOrderBasicSealPostEntity.MAIN_SHAFT_FORM;
                innerOrderbBasicSealMstEntity.First().PUMP_STEERING = innerOrderBasicSealPostEntity.PUMP_STEERING;
                innerOrderbBasicSealMstEntity.First().ASSEMBLE_DIRECTION = innerOrderBasicSealPostEntity.ASSEMBLE_DIRECTION;
                innerOrderbBasicSealMstEntity.First().LINE_LOCATION_DESCRIPTION = innerOrderBasicSealPostEntity.LINE_LOCATION_DESCRIPTION;
                innerOrderbBasicSealMstEntity.First().DRAWING_CONFIRM = innerOrderBasicSealPostEntity.DRAWING_CONFIRM;
                innerOrderbBasicSealMstEntity.First().LUBRICATING_FORM = innerOrderBasicSealPostEntity.LUBRICATING_FORM;

                innerOrderOtherComponentMstEntity.First().BASE_TYPE = innerOrderOtherComponentPostEntity.BASE_TYPE;
                innerOrderOtherComponentMstEntity.First().BASE_SPEC = innerOrderOtherComponentPostEntity.BASE_SPEC;
                innerOrderOtherComponentMstEntity.First().SPECIAL_BASE_DETAIL = innerOrderOtherComponentPostEntity.SPECIAL_BASE_DETAIL;
                innerOrderOtherComponentMstEntity.First().COUPLING_HOOD_TYPE = innerOrderOtherComponentPostEntity.COUPLING_HOOD_TYPE;
                innerOrderOtherComponentMstEntity.First().COUPLING_HOOD_SPEC = innerOrderOtherComponentPostEntity.COUPLING_HOOD_SPEC;
                innerOrderOtherComponentMstEntity.First().SPECIAL_HOOD_TYPE_DETAIL = innerOrderOtherComponentPostEntity.SPECIAL_HOOD_TYPE_DETAIL;
                innerOrderOtherComponentMstEntity.First().ANCHOR_BOLT_TYPE = innerOrderOtherComponentPostEntity.ANCHOR_BOLT_TYPE;
                innerOrderOtherComponentMstEntity.First().ANCHOR_BOLT_SPEC = innerOrderOtherComponentPostEntity.ANCHOR_BOLT_SPEC;
                innerOrderOtherComponentMstEntity.First().ANCHOR_BOLT_MATERIAL = innerOrderOtherComponentPostEntity.ANCHOR_BOLT_MATERIAL;
                innerOrderOtherComponentMstEntity.First().ANCHOR_BOLT_EXTRA_NUT_SPEC = innerOrderOtherComponentPostEntity.ANCHOR_BOLT_EXTRA_NUT_SPEC;
                innerOrderOtherComponentMstEntity.First().ANCHOR_BOLT_EXTRA_PAD_SPEC = innerOrderOtherComponentPostEntity.ANCHOR_BOLT_EXTRA_PAD_SPEC;
                innerOrderOtherComponentMstEntity.First().ANCHOR_BOLT_NUM = innerOrderOtherComponentPostEntity.ANCHOR_BOLT_NUM;
                innerOrderOtherComponentMstEntity.First().COUPLING_TYPE = innerOrderOtherComponentPostEntity.COUPLING_TYPE;
                innerOrderOtherComponentMstEntity.First().COUPLING_BUMP_COUPLET = innerOrderOtherComponentPostEntity.COUPLING_BUMP_COUPLET;
                innerOrderOtherComponentMstEntity.First().COUPLING_ELECTRIC_COUPLET = innerOrderOtherComponentPostEntity.COUPLING_ELECTRIC_COUPLET;
                innerOrderOtherComponentMstEntity.First().COUPLING_PIN = innerOrderOtherComponentPostEntity.COUPLING_PIN;
                innerOrderOtherComponentMstEntity.First().COUPLING_JUMP_RING = innerOrderOtherComponentPostEntity.COUPLING_JUMP_RING;
                innerOrderOtherComponentMstEntity.First().COUPLING_PROVIDER = innerOrderOtherComponentPostEntity.COUPLING_PROVIDER;
                innerOrderOtherComponentMstEntity.First().COUPLING_SPEC = innerOrderOtherComponentPostEntity.COUPLING_SPEC;
                innerOrderOtherComponentMstEntity.First().COUPLING_NUM = innerOrderOtherComponentPostEntity.COUPLING_NUM;
                innerOrderOtherComponentMstEntity.First().SPECIAL_COUPLING_TYPE_DETAIL = innerOrderOtherComponentPostEntity.SPECIAL_COUPLING_TYPE_DETAIL;
                innerOrderOtherComponentMstEntity.First().ELECTRIC_MOTER_TYPE = innerOrderOtherComponentPostEntity.ELECTRIC_MOTER_TYPE;
                innerOrderOtherComponentMstEntity.First().ELECTRIC_MOTER_PROVIDER = innerOrderOtherComponentPostEntity.ELECTRIC_MOTER_PROVIDER;
                innerOrderOtherComponentMstEntity.First().ELECTRIC_MOTER_POWER = innerOrderOtherComponentPostEntity.ELECTRIC_MOTER_POWER;
                innerOrderOtherComponentMstEntity.First().ELECTRIC_MOTER_SPEED = innerOrderOtherComponentPostEntity.ELECTRIC_MOTER_SPEED;
                innerOrderOtherComponentMstEntity.First().ELECTRIC_MOTER_PFV = innerOrderOtherComponentPostEntity.ELECTRIC_MOTER_PFV;
                innerOrderOtherComponentMstEntity.First().ELECTRIC_MOTER_EXTRA_INFO = innerOrderOtherComponentPostEntity.ELECTRIC_MOTER_EXTRA_INFO;
                innerOrderOtherComponentMstEntity.First().COLOR_TYPE = innerOrderOtherComponentPostEntity.COLOR_TYPE;
                innerOrderOtherComponentMstEntity.First().SPECIAL_COLOR_DETAIL = innerOrderOtherComponentPostEntity.SPECIAL_COLOR_DETAIL;
                innerOrderOtherComponentMstEntity.First().SURFACE_TREAT_TYPE = innerOrderOtherComponentPostEntity.SURFACE_TREAT_TYPE;
                innerOrderOtherComponentMstEntity.First().SURFACE_TREAT_EXTRA_INFO = innerOrderOtherComponentPostEntity.SURFACE_TREAT_EXTRA_INFO;
                innerOrderOtherComponentMstEntity.First().TRANSPORT_TYPE = innerOrderOtherComponentPostEntity.TRANSPORT_TYPE;
                innerOrderOtherComponentMstEntity.First().TRANSPORT_PLACE = innerOrderOtherComponentPostEntity.TRANSPORT_PLACE;
                innerOrderOtherComponentMstEntity.First().PACKAGING_TYPE = innerOrderOtherComponentPostEntity.PACKAGING_TYPE;
                innerOrderOtherComponentMstEntity.First().SPECIAL_PACK_DETAIL = innerOrderOtherComponentPostEntity.SPECIAL_PACK_DETAIL;
                innerOrderOtherComponentMstEntity.First().NEED_FUME_CERTIFICATE = innerOrderOtherComponentPostEntity.NEED_FUME_CERTIFICATE;
                innerOrderOtherComponentMstEntity.First().ADDRESS_INFO = innerOrderOtherComponentPostEntity.ADDRESS_INFO;
                innerOrderOtherComponentMstEntity.First().REMARK = innerOrderOtherComponentPostEntity.REMARK;

                myContext.SaveChanges();
                return "";
            }
        }

        // GET api/innerOrder/modelLibrary/{BUMP_INFO}
        [HttpGet("modelLibrary/{BUMP_TYPE}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> GetModelLibrary(string BUMP_TYPE)
        {
            BUMP_TYPE = BUMP_TYPE.Replace("|SLASH|", "/");
            String returnJson = String.Empty;
            if (!String.IsNullOrEmpty(BUMP_TYPE))
            {
                var tempObj = JsonConvert.SerializeObject(myContext.OTHER_COMPONENT_MODEL_MST.Where(d => d.BUMP_TYPE == BUMP_TYPE));

                if (tempObj != "[]")
                {
                    returnJson = JsonConvert.SerializeObject(myContext.OTHER_COMPONENT_MODEL_MST.Where(d => d.BUMP_TYPE == BUMP_TYPE).First());
                }
            }
            return returnJson;
        }

        // GET api/innerOrder/BOMGridData/
        [HttpGet("BOMGridData")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> Get()
        {
            InnerOrderGridObject gridObj = new InnerOrderGridObject();
            gridObj.innerOrderGridBomItemBase = myContext.BOM_ITEM_BASE.ToList();
            gridObj.innerOrderGridBomItemStandard = myContext.BOM_ITEM_STANDARD.ToList();
            String returnJson = JsonConvert.SerializeObject(gridObj);
            // 清空所有项目
            myContext.RemoveRange(myContext.Set<BOM_ITEM_BASE>());
            myContext.RemoveRange(myContext.Set<BOM_ITEM_STANDARD>());
            myContext.SaveChanges();
            return returnJson;
        }

        // GET api/innerOrder/BOMGridData/{BUMP_INFO}
        [HttpGet("BOMGridData/{BUMP_INFO}")]
        [EnableCors("CorsPolicy")]
        public ActionResult<string> GetBOMDataForDisplay(string BUMP_INFO)
        {

            String returnJson = String.Empty;
            if (!String.IsNullOrEmpty(BUMP_INFO))
            {
                BUMP_INFO = BUMP_INFO.Replace("|SLASH|", "/");
                string[] ORDER_NO_AND_BUMP_IDS = BUMP_INFO.Split("|DASH|");
                string BUMP_ID_T = String.Empty;
                string ORDER_NO_T = String.Empty;
                InnerOrderGridObjectForDisplay gridObj = new InnerOrderGridObjectForDisplay();
                if (ORDER_NO_AND_BUMP_IDS.Length > 1)
                {
                    ORDER_NO_T = ORDER_NO_AND_BUMP_IDS[0];
                    BUMP_ID_T = ORDER_NO_AND_BUMP_IDS[1];

                    gridObj.innerOrderGridBomItemBase = myContext.INNER_ORDER_BOM_ITEM_BASE
                                    .Where(d => d.ORDER_NO == ORDER_NO_T)
                                    .Where(d => d.BUMP_ID == BUMP_ID_T).ToList();

                    gridObj.innerOrderGridBomItemStandard = myContext.INNER_ORDER_BOM_ITEM_STANDARD
                                    .Where(d => d.ORDER_NO == ORDER_NO_T)
                                    .Where(d => d.BUMP_ID == BUMP_ID_T).ToList();
                    returnJson = JsonConvert.SerializeObject(gridObj);
                }
            }
            return returnJson;
        }
    }
}